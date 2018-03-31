using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.Interfaces.Model
{
    public interface IModelHighlightOptions : IModelNode
    {
        [System.ComponentModel.Category("Llamachant Framework")]
        bool ShowCountsInTabs { get; set; }
        [System.ComponentModel.Category("Llamachant Framework")]
        bool BoldTabsWithCounts { get; set; }
    }
}
