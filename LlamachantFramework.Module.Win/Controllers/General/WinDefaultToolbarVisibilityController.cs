using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Win.SystemModule;
using LlamachantFramework.Module.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.Win.Controllers.General
{
    public class WinDefaultToolbarVisibilityController : DefaultToolbarVisibilityController
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            this.Frame.GetController<ToolbarVisibilityController>().ShowToolbarAction.Active[this.Name] = (this.View.Model as IModelToolbarVisibility).AllowToolbarVisibilityToggle;
        }
        protected override void SetToolbarVisibility(bool visible)
        {
            ISupportActionsToolbarVisibility template = this.Frame.Template as ISupportActionsToolbarVisibility;
            if (template != null)
                template.SetVisible(visible);
        }
    }
}
