using DevExpress.Data.Filtering.Exceptions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.Controllers.Utils
{
    public class TrackedObjectHelper
    {
        public static void AfterConstruction(ITrackedObject obj, string createdbyproperty)
        {
            obj.CreatedOn = DateTime.Now;
            SetUserProperty(obj, createdbyproperty);
        }

        public static void OnSaving(ITrackedObject obj, string modifiedbyproperty)
        {
            obj.ModifiedOn = DateTime.Now;
            SetUserProperty(obj, modifiedbyproperty);
        }

        public static void OnDeleting(ITrackedObject obj, string deletedbyproperty)
        {
            obj.DeletedOn = DateTime.Now;
            SetUserProperty(obj, deletedbyproperty);
        }

        private static void SetUserProperty(ITrackedObject obj, string property)
        {
            if (string.IsNullOrEmpty(property))
                return;

            IMemberInfo member = XafTypesInfo.Instance.FindTypeInfo(obj.GetType()).FindMember(property);
            if (member == null)
                throw new InvalidPropertyPathException(string.Format("The {0} property does not existing within the {1} type", property, obj.GetType().Name));

            member.SetValue(obj, obj.Session.GetObjectByKey(SecuritySystem.CurrentUser.GetType(), SecuritySystem.CurrentUserId));
        }
    }

    public interface ITrackedObject
    {
        DateTime CreatedOn { get; set; }
        DateTime ModifiedOn { get; set; }
        DateTime DeletedOn { get; set; }

        Session Session { get; }
    }
}
