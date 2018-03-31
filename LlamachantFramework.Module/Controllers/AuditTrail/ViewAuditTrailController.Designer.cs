namespace LlamachantFramework.Module.Controllers.AuditTrail
{
    partial class ViewAuditTrailController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.actionViewAuditTrail = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // actionViewAuditTrail
            // 
            this.actionViewAuditTrail.AcceptButtonCaption = null;
            this.actionViewAuditTrail.CancelButtonCaption = null;
            this.actionViewAuditTrail.Caption = "View Audit Trail";
            this.actionViewAuditTrail.Category = "Tools";
            this.actionViewAuditTrail.ConfirmationMessage = null;
            this.actionViewAuditTrail.Id = "actionViewAuditTrail";
            this.actionViewAuditTrail.ImageName = "BO_Audit_ChangeHistory";
            this.actionViewAuditTrail.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.actionViewAuditTrail.ToolTip = null;
            this.actionViewAuditTrail.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.actionViewAuditTrail_CustomizePopupWindowParams);
            // 
            // ViewAuditTrailController
            // 
            this.Actions.Add(this.actionViewAuditTrail);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction actionViewAuditTrail;
    }
}
