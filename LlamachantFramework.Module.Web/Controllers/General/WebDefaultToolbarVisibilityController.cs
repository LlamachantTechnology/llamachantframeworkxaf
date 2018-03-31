using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using LlamachantFramework.Module.Controllers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace LlamachantFramework.Module.Web.Controllers.General
{
    public class WebDefaultToolbarVisibilityController : DefaultToolbarVisibilityController
    {
        protected override void SetToolbarVisibility(bool visible)
        {
            if (Frame.Template != null)
            {
                foreach (ActionContainerHolder holder in FindActionHolders((Control)Frame.Template))
                {
                    string current = holder.Style[HtmlTextWriterStyle.Display];
                    holder.Style[HtmlTextWriterStyle.Visibility] = visible ? current : "Hidden";
                    holder.Parent.Visible = visible;
                }
            }
        }

        private List<ActionContainerHolder> FindActionHolders(Control c)
        {
            List<ActionContainerHolder> holders = new List<ActionContainerHolder>();
            foreach (Control control in c.Controls)
            {
                if (control is ActionContainerHolder)
                    holders.Add((ActionContainerHolder)control);
                holders.AddRange(FindActionHolders(control));
            }

            return holders;
        }
    }
}
