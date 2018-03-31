using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Layout;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraLayout;
using LlamachantFramework.Module.Interfaces.Model;

namespace LlamachantFramework.Module.Controllers.General
{
    public class WinHighlightTabsController : ViewController<DetailView>//HighlightTabsController
    {
        public WinHighlightTabsController()
        {

        }

        public IModelHighlightOptions HighlightOptions { get { return Application.Model.Options as IModelHighlightOptions; } }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            RefreshLayoutControl();
            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
            View.CurrentObjectChanged += View_CurrentObjectChanged;
        }
        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e)
        {
            RefreshLayoutControl();
        }
        private void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            if (View.CurrentObject != null)
            {
                RefreshLayoutControl();
            }
        }
        protected override void OnDeactivated()
        {
            View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            ObjectSpace.ModifiedChanged -= new EventHandler(ObjectSpace_ModifiedChanged);

            base.OnDeactivated();
        }

        private void RefreshLayoutControl()
        {
            LayoutControl layoutControl = (LayoutControl)((DetailView)View).LayoutManager.Container;
            HighlightTabs(layoutControl.Root, Application, (DetailView)View);
        }


        public void HighlightTabs(LayoutItemContainer group, XafApplication application, DetailView view)
        {
            group.BeginUpdate();
            LayoutGroup layoutGroup = group as LayoutGroup;
            if ((layoutGroup != null) && layoutGroup.IsInTabbedGroup)
            {
                bool isBold = false;
                foreach (BaseLayoutItem item in layoutGroup.Items)
                {
                    LayoutControlItem layoutControlItem = item as LayoutControlItem;
                    if (layoutControlItem == null)
                    {
                        continue;
                    }
                    PropertyEditor propertyEditor = FindPropertyEditor(view, layoutControlItem);
                    object currentValue = propertyEditor != null ? propertyEditor.MemberInfo.GetValue(propertyEditor.CurrentObject) : null;

                    int index = layoutGroup.Text.IndexOf('(');
                    if (index != -1)
                    {
                        if (layoutGroup.Text.EndsWith(")"))
                            layoutGroup.Text = layoutGroup.Text.Substring(0, index - 1);
                    }

                    if (HighlightOptions.ShowCountsInTabs)
                    {
                        ICollection collection = currentValue as ICollection;
                        if (collection != null)
                        {
                            int count = collection.Count;
                            isBold = count > 0 && HighlightOptions.BoldTabsWithCounts;

                            if (count != 0)
                            {
                                layoutGroup.Text = String.Format("{1} ({0})", count, layoutGroup.Text);
                            }
                        }
                        else
                        {
                            isBold = currentValue != null && !String.IsNullOrEmpty(currentValue.ToString()) && HighlightOptions.BoldTabsWithCounts;
                        }
                        if (isBold)
                            break;
                    }
                }
                Font font = layoutGroup.AppearanceTabPage.Header.Font;

                layoutGroup.AppearanceTabPage.Header.Font = new Font(font, isBold ? FontStyle.Bold : FontStyle.Regular);
                layoutGroup.AppearanceTabPage.HeaderHotTracked.Font = new Font(font, isBold ? FontStyle.Bold : FontStyle.Regular);
            }
            IEnumerable items;
            if (group is TabbedGroup)
            {
                items = ((TabbedGroup)group).TabPages;
            }
            else
            {
                items = ((LayoutGroup)group).Items;
            }
            foreach (BaseLayoutItem item in items)
            {
                if (item is LayoutItemContainer)
                {
                    HighlightTabs((LayoutItemContainer)item, application, view);
                }
            }
            group.EndUpdate();
        }

        private PropertyEditor FindPropertyEditor(DetailView view, LayoutControlItem layoutItem)
        {
            if (layoutItem is XafLayoutControlItem)
            {
                return (view.LayoutManager as WinLayoutManager).FindViewItem(layoutItem) as PropertyEditor;
            }
            return null;
        }
    }
}
