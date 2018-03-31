namespace LlamachantFramework.Module.Win.Controllers.General
{
    partial class SvgSkinController
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
            this.actionThemeOptions = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // actionThemeOptions
            // 
            this.actionThemeOptions.Caption = "Theme Options";
            this.actionThemeOptions.Category = "Windows";
            this.actionThemeOptions.ConfirmationMessage = null;
            this.actionThemeOptions.Id = "actionThemeOptions";
            this.actionThemeOptions.ImageName = "Action_ThemeOptions";
            this.actionThemeOptions.ToolTip = null;
            this.actionThemeOptions.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.actionThemeOptions_Execute);
            // 
            // SvgSkinController
            // 
            this.Actions.Add(this.actionThemeOptions);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction actionThemeOptions;
    }
}
