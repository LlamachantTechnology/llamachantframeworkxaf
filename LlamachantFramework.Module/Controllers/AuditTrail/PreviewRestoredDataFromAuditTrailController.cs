using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using LlamachantFramework.Module.Controllers.Utils;

namespace LlamachantFramework.Module.Controllers.AuditTrail
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PreviewRestoredDataFromAuditTrailController : ViewController
    {
        public PreviewRestoredDataFromAuditTrailController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            this.TargetObjectType = typeof(RestoreDataParameters);
            this.TargetViewType = ViewType.DetailView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void actionPreviewRestoredData_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            ListPropertyEditor editor = (this.View as DetailView).FindItem("DeletedItems") as ListPropertyEditor;

            if (editor != null)
            {
                IObjectSpace space = Application.CreateObjectSpace();
                RestoredObjectsParameters p = new RestoredObjectsParameters();
                using (AuditTrailRestoreHelper helper = new AuditTrailRestoreHelper(space))
                {
                    foreach (RestoreItemDetails details in editor.ListView.SelectedObjects)
                        helper.RestoreObject(space.GetObject<AuditDataItemPersistent>(details.AuditTrailItem));

                    foreach (object obj in helper.RestoredObjects)
                        p.ObjectsToRestore.Add(new RestoredObjectDetails() { Name = CaptionHelper.GetDisplayText(obj), Type = CaptionHelper.GetClassCaption(XafTypesInfo.Instance.FindTypeInfo(obj.GetType()).Type.FullName) });
                }

                IObjectSpace previewspace = Application.CreateObjectSpace(typeof(RestoredObjectsParameters));
                e.View = Application.CreateDetailView(previewspace, p);
                e.DialogController.SaveOnAccept = false;
            }
        }
    }

    [ImageName("Trash")]
    [DomainComponent]
    public class RestoredObjectsParameters
    {
        public RestoredObjectsParameters()
        {
            ObjectsToRestore = new List<RestoredObjectDetails>();
        }
        public List<RestoredObjectDetails> ObjectsToRestore { get; private set; }
    }
    [ImageName("Trash")]
    [DomainComponent]
    public class RestoredObjectDetails
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
