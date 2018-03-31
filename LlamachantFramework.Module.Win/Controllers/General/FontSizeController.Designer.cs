namespace LlamachantFramework.Module.Win.Controllers.General
{
    partial class FontSizeController
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
            this.actionChooseFontSize = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            // 
            // actionChooseFontSize
            // 
            this.actionChooseFontSize.Caption = "Font Size";
            this.actionChooseFontSize.Category = "Windows";
            this.actionChooseFontSize.ConfirmationMessage = null;
            this.actionChooseFontSize.Id = "actionChooseFontSize";
            this.actionChooseFontSize.ImageName = "Action_FontSize";
            this.actionChooseFontSize.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.actionChooseFontSize.ToolTip = null;
            this.actionChooseFontSize.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.actionChooseFontSize_Execute);
            // 
            // FontSizeController
            // 
            this.Actions.Add(this.actionChooseFontSize);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SingleChoiceAction actionChooseFontSize;
    }
}
