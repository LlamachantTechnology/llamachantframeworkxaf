using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.AuditTrail;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LlamachantFramework.Module.Controllers.Utils
{
    public class AuditTrailRestoreHelper : IDisposable
    {
        public List<object> RestoredObjects { get; private set; }
        public IObjectSpace ObjectSpace { get; private set; }

        private List<object> aggregateChecked = new List<object>();        

        public AuditTrailRestoreHelper(IObjectSpace space)
        {
            ObjectSpace = space;
            RestoredObjects = new List<object>();
        }

        public void RestoreObject(AuditDataItemPersistent audit)
        {
            object currentobj = audit.AuditedObject.Target;

            if (!RestoredObjects.Contains(currentobj))
                RestoredObjects.Add(currentobj);
            else
                return;

            ITypeInfo currenttypeinfo = XafTypesInfo.Instance.FindTypeInfo(currentobj.GetType());
            foreach (AuditDataItemPersistent item in ObjectSpace.CreateCollection(typeof(AuditDataItemPersistent), CriteriaOperator.Parse("(AuditedObject = ? OR OldObject.TargetKey = ?) AND ModifiedOn >= ? AND (OperationType = 'ObjectDeleted' OR OperationType = 'RemovedFromCollection')", audit.AuditedObject, XPWeakReference.KeyToString(currenttypeinfo.KeyMember.GetValue(currentobj)), audit.ModifiedOn), new List<SortProperty>() { new SortProperty("ModifiedOn", DevExpress.Xpo.DB.SortingDirection.Ascending) }))
            {
                if (item.OperationType == "ObjectDeleted")
                {
                    UndeleteObjectCore(currentobj, currenttypeinfo);
                }
                else if (item.OperationType == "RemovedFromCollection")
                {
                    object oldobj = item.OldObject.Target;
                    UndeleteObject(oldobj);

                    object associatedobject = item.AuditedObject.Target;
                    UndeleteObject(associatedobject);

                    ITypeInfo associatedobjectinfo = XafTypesInfo.Instance.FindTypeInfo(associatedobject.GetType());
                    IList collection = associatedobjectinfo.FindMember(item.PropertyName).GetValue(associatedobject) as IList;
                    collection.Add(oldobj);

                    RestoreAggregateObjects(oldobj);
                    RestoreAggregateObjects(associatedobject);
                }
            }

            RestoreAggregateObjects(currentobj);
        }

        private void UndeleteObject(object obj)
        {
            if (obj == null || RestoredObjects.Contains(obj))
                return;

            ITypeInfo objinfo = XafTypesInfo.Instance.FindTypeInfo(obj.GetType());
            UndeleteObjectCore(obj, objinfo);
            XPCollection<AuditDataItemPersistent> originalcollection = AuditedObjectWeakReference.GetAuditTrail((ObjectSpace as XPObjectSpace).Session, obj);
            if (originalcollection != null)
            {
                XPCollection<AuditDataItemPersistent> oldobjaudit = new XPCollection<AuditDataItemPersistent>(originalcollection, new BinaryOperator("OperationType", "ObjectDeleted"));
                if (oldobjaudit.Count > 0)
                    RestoreObject(oldobjaudit.OrderByDescending(x => x.ModifiedOn).First());
            }
        }

        private void UndeleteObjectCore(object obj, ITypeInfo currenttypeinfo)
        {
            if (currenttypeinfo.IsDomainComponent)
            {
                System.Reflection.MethodInfo method = obj.GetType().GetMethod("SetMemberValue");
                method.Invoke(obj, new object[] { "GCRecord", null });
            }
            else if (currenttypeinfo.FindAttribute<DeferredDeletionAttribute>() != null && currenttypeinfo.FindMember("GCRecord").GetValue(obj) != null)
                currenttypeinfo.FindMember("GCRecord").SetValue(obj, null);
        }

        private void RestoreAggregateObjects(object obj)
        {
            if (aggregateChecked.Contains(obj))
                return;

            aggregateChecked.Add(obj);

            ITypeInfo info = XafTypesInfo.Instance.FindTypeInfo(obj.GetType());
            foreach (IMemberInfo member in info.Members.Where(x => x.IsAggregated && !x.IsList))
            {
                object value = member.GetValue(obj);
                UndeleteObject(value);
            }
        }

        public void MarkAsRestored()
        {
            foreach (object obj in RestoredObjects)
            {
                AuditDataItem a = new AuditDataItem(obj, null, "Deleted", "Restored", AuditOperationType.CustomData);
                AuditTrailService.Instance.AddCustomAuditData((ObjectSpace as XPObjectSpace).Session, a);
            }
        }

        public void Dispose()
        {
            aggregateChecked.Clear();
            RestoredObjects.Clear();
            ObjectSpace = null;
        }
    }
}
