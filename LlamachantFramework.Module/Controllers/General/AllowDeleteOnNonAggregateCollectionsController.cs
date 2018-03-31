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
    public class AllowDeleteOnNonAggregateCollectionsController : ViewController<ListView>, IModelExtender
    {
        public AllowDeleteOnNonAggregateCollectionsController() { }

        protected override void OnActivated()
        {
            base.OnActivated();

            PropertyCollectionSource cs = this.View.CollectionSource as PropertyCollectionSource;
            this.Frame.GetController<DeleteObjectsViewController>().DeleteAction.Active[this.Name] = cs == null || (cs.MemberInfo.IsManyToMany && cs.MemberInfo.IsAggregated) || ((this.View.Model as IModelAllowDeleteOnNonAggregatedCollections).AllowDeleteNonAggregatedObject && (Application.Model.Options as IModelDefaultAllowDeleteOnNonAggregatedCollections).AllowDeleteNonAggregatedObjects) || ((this.View.Model as IModelAllowDeleteOnNonAggregatedCollections).HasValue("AllowDeleteNonAggregatedObject") && (this.View.Model as IModelAllowDeleteOnNonAggregatedCollections).AllowDeleteNonAggregatedObject);
        }



        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelOptions, IModelDefaultAllowDeleteOnNonAggregatedCollections>();
            extenders.Add<IModelListView, IModelAllowDeleteOnNonAggregatedCollections>();
        }
    }

    public interface IModelAllowDeleteOnNonAggregatedCollections : IModelNode
    {
        [Category("Llamachant Framework")]
        [DefaultValue(true)]
        bool AllowDeleteNonAggregatedObject { get; set; }
    }

    public interface IModelDefaultAllowDeleteOnNonAggregatedCollections : IModelNode
    {
        [Category("Llamachant Framework")]
        [DefaultValue(true)]
        bool AllowDeleteNonAggregatedObjects { get; set; }
    }
}
