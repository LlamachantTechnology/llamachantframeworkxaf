using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using LlamachantFramework.Module.Controllers.Utils;
using LlamachantFramework.Module.Interfaces;

namespace LlamachantFramework.Module.Controllers.AuditTrail
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppWindowControllertopic.aspx.
    public partial class RestoreDataFromAuditTrailController : WindowController
    {
        public RestoreDataFromAuditTrailController()
        {
            InitializeComponent();
            // Target required Windows (via the TargetXXX properties) and create their Actions.
            this.TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target Window.
            SetAuditTrailActionVisibility();
        }

        private void SetAuditTrailActionVisibility()
        {
            IModelAuditTrailOptions options = Application.Model.Options as IModelAuditTrailOptions;
            this.actionRestoreData.Active[this.Name] = ViewAuditTrailController.AuditTrailEnabled.Value && (options.CanRestoreFromAuditTrail == AuditTrailOption.All ||
                (options.CanRestoreFromAuditTrail == AuditTrailOption.UserSpecific && SecuritySystem.CurrentUser is IAuditTrailUser && ((IAuditTrailUser)SecuritySystem.CurrentUser).CanRestoreFromAuditTrail));
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void actionRestoreData_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace space = Application.CreateObjectSpace();
            RestoreDataParameters p = space.CreateObject<RestoreDataParameters>();
            e.View = Application.CreateDetailView(space, p);
            (e.View as DetailView).ViewEditMode = ViewEditMode.Edit;
        }

        private void actionRestoreData_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            RestoreDataParameters p = e.PopupWindowViewCurrentObject as RestoreDataParameters;
            DetailView view = e.PopupWindowView as DetailView;
            ListPropertyEditor editor = view.FindItem("DeletedItems") as ListPropertyEditor;
            
            IObjectSpace space = Application.CreateObjectSpace();
            using (AuditTrailRestoreHelper helper = new AuditTrailRestoreHelper(space))
            {
                foreach (RestoreItemDetails details in editor.ListView.SelectedObjects)
                    helper.RestoreObject(space.GetObject<AuditDataItemPersistent>(details.AuditTrailItem));

                helper.MarkAsRestored();
                space.CommitChanges();
            }
        }
    }

    [ImageName("Trash")]
    [NonPersistent]
    public class RestoreDataParameters : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public RestoreDataParameters(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private bool updaterequired = true;

        private DateTime _SearchFrom;
        [ImmediatePostData]
        public DateTime SearchFrom
        {
            get { return _SearchFrom; }
            set
            {
                SetPropertyValue<DateTime>(nameof(SearchFrom), ref _SearchFrom, value);
                RequestUpdate();
            }
        }


        private DateTime _SearchTo;
        [ImmediatePostData]
        public DateTime SearchTo
        {
            get { return _SearchTo; }
            set
            {
                SetPropertyValue<DateTime>(nameof(SearchTo), ref _SearchTo, value);
                RequestUpdate();
            }
        }


        private string _SearchDisplayName;
        public string SearchDisplayName
        {
            get { return _SearchDisplayName; }
            set
            {
                SetPropertyValue<string>(nameof(SearchDisplayName), ref _SearchDisplayName, value);
                RequestUpdate();
            }
        }

        List<RestoreItemDetails> details = new List<RestoreItemDetails>();
        [NonPersistent]
        public List<RestoreItemDetails> DeletedItems
        {
            get
            {
                if (updaterequired && (!string.IsNullOrEmpty(SearchDisplayName) || (SearchFrom != DateTime.MinValue && SearchTo != DateTime.MinValue)))
                {
                    details.Clear();

                    List<CriteriaOperator> criteria = new List<CriteriaOperator>() { new BinaryOperator("OperationType", "ObjectDeleted"), CriteriaOperator.Parse("[AuditedObject] is not null") };
                    if (!String.IsNullOrEmpty(SearchDisplayName))
                        criteria.Add(CriteriaOperator.Parse("Contains([Description], ?)", SearchDisplayName));

                    if (SearchFrom != DateTime.MinValue && SearchTo != DateTime.MinValue)
                        criteria.Add(new BetweenOperator("ModifiedOn", SearchFrom, SearchTo.AddDays(1)));

                    using (XPCollection<AuditDataItemPersistent> collection = new XPCollection<AuditDataItemPersistent>(Session, CriteriaOperator.And(criteria), new SortProperty[] { new SortProperty("ModifiedOn", DevExpress.Xpo.DB.SortingDirection.Descending) }))
                    {
                        foreach (AuditDataItemPersistent item in collection)
                        {
                            if (details.Where(x => x.AuditTrailItem.AuditedObject == item.AuditedObject).Count() == 0)
                            {
                                details.Add(new RestoreItemDetails(Session) { AuditTrailItem = item, Name = item.AuditedObject.DisplayName, TypeName = CaptionHelper.GetClassCaption(XafTypesInfo.Instance.FindTypeInfo(item.AuditedObject.Target.GetType()).Type.FullName), DeletedOn = item.ModifiedOn, DeletedBy = item.UserName, Restored = !Session.IsObjectMarkedDeleted(item.AuditedObject.Target) });
                            }
                        }
                    }

                    updaterequired = false;
                }

                return details;
            }
        }

        private void RequestUpdate()
        {
            if (!IsSaving && !IsLoading)
            {
                updaterequired = true;
                OnChanged("DeletedItems");
            }
        }
    }

    [NonPersistent]
    [Appearance("Restored", "[Restored] = True", TargetItems = "*", FontStyle = FontStyle.Italic)]
    public class RestoreItemDetails : BaseObject
    {
        public RestoreItemDetails(Session session) : base(session)
        {

        }

        private string _TypeName;
        public string TypeName
        {
            get { return _TypeName; }
            set { SetPropertyValue<string>(nameof(TypeName), ref _TypeName, value); }
        }


        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>(nameof(Name), ref _Name, value); }
        }


        private DateTime _DeletedOn;
        public DateTime DeletedOn
        {
            get { return _DeletedOn; }
            set { SetPropertyValue<DateTime>(nameof(DeletedOn), ref _DeletedOn, value); }
        }


        private string _DeletedBy;
        public string DeletedBy
        {
            get { return _DeletedBy; }
            set { SetPropertyValue<string>(nameof(DeletedBy), ref _DeletedBy, value); }
        }


        private bool _Restored;
        public bool Restored
        {
            get { return _Restored; }
            set { SetPropertyValue<bool>(nameof(Restored), ref _Restored, value); }
        }


        private AuditDataItemPersistent _AuditTrailItem;
        [Browsable(false)]
        public AuditDataItemPersistent AuditTrailItem
        {
            get { return _AuditTrailItem; }
            set { SetPropertyValue<AuditDataItemPersistent>(nameof(AuditTrailItem), ref _AuditTrailItem, value); }
        }
    }
}
