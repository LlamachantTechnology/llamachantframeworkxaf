using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Layout;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using LlamachantFramework.Module.Controllers.General;
using LlamachantFramework.Module.Interfaces.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace LlamachantFramework.Module.Web.Controllers.General
{
    public class WebHighlightTabsController : ViewController<DetailView>
    {
        private List<ASPxTabControlBase> tabControls = new List<ASPxTabControlBase>();

        public WebHighlightTabsController() { }

        internal WebLayoutManager LayoutManager { get { return View.LayoutManager as WebLayoutManager; } }
        public IModelHighlightOptions HighlightOptions { get { return Application.Model.Options as IModelHighlightOptions; } }

        protected override void OnActivated()
        {
            base.OnActivated();

            if (HighlightOptions.ShowCountsInTabs)
            {
                LayoutManager.ItemCreated += LayoutManager_ItemCreated;
                View.CurrentObjectChanged += View_CurrentObjectChanged;
            }
        }

        protected override void OnDeactivated()
        {
            if (HighlightOptions.ShowCountsInTabs)
            {
                LayoutManager.ItemCreated -= LayoutManager_ItemCreated;
                View.CurrentObjectChanged -= View_CurrentObjectChanged;
            }

            base.OnDeactivated();
        }
        
        protected override void OnViewControlsDestroying()
        {
            tabControls.Clear();
            base.OnViewControlsDestroying();
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            if (View.CurrentObject != null)
            {
                foreach (ASPxTabControlBase tabcontrol in tabControls)
                {
                    CustomizeASPxTabControl(tabcontrol);
                }
            }
        }

        private void LayoutManager_ItemCreated(object sender, ItemCreatedEventArgs e)
        {
            if (e.ModelLayoutElement is IModelTabbedGroup && e.TemplateContainer != null)
                e.TemplateContainer.Load += TemplateContainer_Load;
        }

        private void TemplateContainer_Load(object sender, EventArgs e)
        {
            Control c = (Control)sender;
            c.Load -= TemplateContainer_Load;
            ASPxTabControlBase tabcontrol = FindFirstControl<ASPxTabControlBase>(c);
            if (tabcontrol != null)
            {
                tabControls.Add(tabcontrol);
                CustomizeASPxTabControl(tabcontrol);
            }
        }

        private T FindFirstControl<T>(Control parent) where T : Control
        {
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                    return (T)control;
            }
            foreach (Control control in parent.Controls)
                return FindFirstControl<T>(control);
            return null;
        }

        private void CustomizeASPxTabControl(ASPxTabControlBase tabControl)
        {
            ASPxPageControl pagecontrol = tabControl as ASPxPageControl;
            if (pagecontrol != null)
            {
                foreach (TabPage page in pagecontrol.TabPages)
                {
                    string propertyname = page.Name;
                    if (propertyname.Contains("_Group"))
                        propertyname = propertyname.Substring(0, propertyname.IndexOf("_Group"));

                    MemberInfo[] members = this.View.CurrentObject.GetType().GetMember(propertyname);
                    if (members.Length > 0)
                    {
                        object obj = XafTypesInfo.Instance.FindTypeInfo(this.View.CurrentObject.GetType()).FindMember(propertyname).GetValue(this.View.CurrentObject);
                        if (obj is ICollection)
                        {
                            if ((obj as ICollection).Count > 0)
                            {
                                page.Text = String.Format("{0} ({1})", this.View.FindItem(propertyname).Caption, (obj as ICollection).Count);
                                page.TabStyle.Font.Bold = HighlightOptions.BoldTabsWithCounts;
                            }
                            else
                            {
                                page.Text = String.Format("{0}", this.View.FindItem(propertyname).Caption);
                                page.TabStyle.Font.Bold = false;
                            }
                        }
                        else if (obj != null)
                        {
                            page.TabStyle.Font.Bold = HighlightOptions.BoldTabsWithCounts;
                        }
                    }
                }
            }
        }
    }
}
