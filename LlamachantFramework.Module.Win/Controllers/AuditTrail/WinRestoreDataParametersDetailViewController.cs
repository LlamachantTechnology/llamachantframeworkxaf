using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using LlamachantFramework.Module.Controllers.AuditTrail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LlamachantFramework.Module.Win.Controllers.AuditTrail
{
    public class WinRestoreDataParametersDetailViewController : ViewController<DetailView>
    {
        StringPropertyEditor editor = null;

        public WinRestoreDataParametersDetailViewController()
        {
            this.TargetObjectType = typeof(RestoreDataParameters);
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            editor = this.View.FindItem("SearchDisplayName") as StringPropertyEditor;

            if (editor != null && editor.Control as StringEdit != null)
                editor.Control.KeyDown += Control_KeyDown;
        }

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();

            DialogController dc = Frame.GetController<DialogController>();
            if(dc != null && dc.AcceptAction != null) {
                dc.AcceptAction.ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Unknown;
            }
        }

        private void Control_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.Enter)
            {
                e.SuppressKeyPress = true;
                editor.WriteValue();
            }
        }

        protected override void OnDeactivated()
        {
            if (editor != null)
                editor.Control.KeyDown -= Control_KeyDown;

            base.OnDeactivated();
        }
    }
}
