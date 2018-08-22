using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using LlamachantFramework.Module.Controllers.AuditTrail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LlamachantFramework.Module.Win.Controllers.AuditTrail
{
    public class WinRestoreAuditDetailsListController : ViewController<ListView>
    {
        public WinRestoreAuditDetailsListController()
        {
            this.TargetObjectType = typeof(RestoreItemDetails);
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            GridListEditor editor = this.View.Editor as GridListEditor;
            if (editor != null)
            {
                editor.GridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
                editor.GridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
            }
        }
    }
}
