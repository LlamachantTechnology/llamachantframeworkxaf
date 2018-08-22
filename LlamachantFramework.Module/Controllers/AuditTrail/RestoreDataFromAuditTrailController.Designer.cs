namespace LlamachantFramework.Module.Controllers.AuditTrail
{
    partial class RestoreDataFromAuditTrailController
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
            this.actionRestoreData = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // actionRestoreData
            // 
            this.actionRestoreData.AcceptButtonCaption = null;
            this.actionRestoreData.CancelButtonCaption = null;
            this.actionRestoreData.Caption = "Restore Data";
            this.actionRestoreData.Category = "Tools";
            this.actionRestoreData.ConfirmationMessage = "Warning: Restoring from the audit trail is experimental. We suggest backing up yo" +
    "ur data before proceeding. Do you want to continue?";
            this.actionRestoreData.Id = "actionRestoreData";
            this.actionRestoreData.ImageName = "Trash";
            this.actionRestoreData.ToolTip = null;
            this.actionRestoreData.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.actionRestoreData_CustomizePopupWindowParams);
            this.actionRestoreData.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.actionRestoreData_Execute);
            // 
            // RestoreDataFromAuditTrailController
            // 
            this.Actions.Add(this.actionRestoreData);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction actionRestoreData;
    }
}
