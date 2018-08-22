namespace LlamachantFramework.Module.Controllers.AuditTrail
{
    partial class PreviewRestoredDataFromAuditTrailController
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
            this.actionPreviewRestoredData = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // actionPreviewRestoredData
            // 
            this.actionPreviewRestoredData.AcceptButtonCaption = null;
            this.actionPreviewRestoredData.CancelButtonCaption = null;
            this.actionPreviewRestoredData.Caption = "Preview";
            this.actionPreviewRestoredData.Category = "PopupActions";
            this.actionPreviewRestoredData.ConfirmationMessage = null;
            this.actionPreviewRestoredData.Id = "actionPreviewRestoredData";
            this.actionPreviewRestoredData.ToolTip = null;
            this.actionPreviewRestoredData.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.actionPreviewRestoredData_CustomizePopupWindowParams);
            // 
            // PreviewRestoredDataFromAuditTrailController
            // 
            this.Actions.Add(this.actionPreviewRestoredData);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction actionPreviewRestoredData;
    }
}
