using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using LlamachantFramework.Module.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.ModelGenerators
{
    public class ISingletonBOModelUpdater : ModelNodesGeneratorUpdater<ModelViewsNodesGenerator>
    {
        public override void UpdateNode(DevExpress.ExpressApp.Model.Core.ModelNode node)
        {
            foreach (IModelView view in node.Nodes)
            {
                IModelObjectView objectview = view.AsObjectView;
                if (objectview != null && objectview.ModelClass.TypeInfo.Implements<ISingletonBO>())
                {
                    view.AllowNew = false;
                    view.AllowDelete = false;
                }
            }
        }
    }
}
