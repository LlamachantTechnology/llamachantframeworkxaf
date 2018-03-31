using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.Controllers.General
{
    public class DefaultToolbarVisibilityController : ViewController<ListView>, IModelExtender
    {
        public DefaultToolbarVisibilityController()
        {
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            SetToolbarVisibility((this.View.Model as IModelToolbarVisibility).ToolbarVisible);
        }

        protected virtual void SetToolbarVisibility(bool visible)
        {

        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelListView, IModelToolbarVisibility>();
        }
    }

    public interface IModelToolbarVisibility : IModelNode
    {
        [Category("Llamachant Framework")]
        [DefaultValue(true)]
        bool ToolbarVisible { get; set; }
        [Category("Llamachant Framework")]
        [DefaultValue(true)]
        bool AllowToolbarVisibilityToggle { get; set; }
    }
}
