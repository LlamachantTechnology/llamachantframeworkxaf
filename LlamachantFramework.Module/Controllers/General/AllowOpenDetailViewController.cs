using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.Controllers.General
{
    public class AllowOpenDetailViewController : ViewController<ListView>, IModelExtender
    {
        public AllowOpenDetailViewController() { }

        protected override void OnActivated()
        {
            base.OnActivated();
            this.Frame.GetController<ListViewProcessCurrentObjectController>().CustomHandleProcessSelectedItem += AllowOpenDetailViewController_CustomHandleProcessSelectedItem;
        }

        private void AllowOpenDetailViewController_CustomHandleProcessSelectedItem(object sender, HandledEventArgs e)
        {
            e.Handled = (this.View.Model as IModelAllowOpenDetailView).AllowOpenDetailView == false;
        }

        protected override void OnDeactivated()
        {
            this.Frame.GetController<ListViewProcessCurrentObjectController>().CustomHandleProcessSelectedItem -= AllowOpenDetailViewController_CustomHandleProcessSelectedItem;
            base.OnDeactivated();
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelListView, IModelAllowOpenDetailView>();
        }
    }

    public interface IModelAllowOpenDetailView : IModelNode
    {
        [Category("Llamachant Framework")]
        [DefaultValue(true)]
        bool AllowOpenDetailView { get; set; }
    }
}
