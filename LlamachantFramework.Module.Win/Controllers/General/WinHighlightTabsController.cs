using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class WinHighlightTabsController : ViewController<DetailView>
    {
        XafLayoutControl layoutControl = null;
        PropertyEditorGroupHelper editorHelper = new PropertyEditorGroupHelper();
        public IModelHighlightOptions HighlightOptions { get { return Application.Model.Options as IModelHighlightOptions; } }

        public WinHighlightTabsController()
        {

        }
        
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            layoutControl = View.Control as XafLayoutControl;
            if (layoutControl != null)
            {
                RefreshLayoutControl();
                View.CurrentObjectChanged += View_CurrentObjectChanged;
                layoutControl.CustomizationClosed += LayoutControl_CustomizationClosed;
                editorHelper.Initialize(View);
                editorHelper.HighlightGroup += EditorHelper_HighlightGroup;
            }
        }

        private void EditorHelper_HighlightGroup(object sender, LayoutGroupEventArgs e)
        {
            HighlightTabs(e.LayoutGroup);
        }

        private void LayoutControl_CustomizationClosed(object sender, EventArgs e)
        {
            editorHelper.Reset();
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
            if (layoutControl != null)
            {
                layoutControl.CustomizationClosed -= LayoutControl_CustomizationClosed;
                View.CurrentObjectChanged -= View_CurrentObjectChanged;
                editorHelper.HighlightGroup -= EditorHelper_HighlightGroup;
                editorHelper.Destroy();
            }

            base.OnDeactivated();
        }


        private void RefreshLayoutControl()
        {
            HighlightTabs(layoutControl.Root);
        }


        public void HighlightTabs(LayoutItemContainer group)
        {
            try
            {
                group.BeginUpdate();

                LayoutGroup layoutGroup = group as LayoutGroup;
                if (layoutGroup != null && layoutGroup.IsInTabbedGroup)
                {
                    bool isBold = false;
                    foreach (BaseLayoutItem item in layoutGroup.Items)
                    {
                        LayoutControlItem layoutControlItem = item as LayoutControlItem;
                        if (layoutControlItem == null)
                            continue;

                        int index = layoutGroup.Text.IndexOf('(');
                        if (index != -1)
                        {
                            if (layoutGroup.Text.EndsWith(")"))
                                layoutGroup.Text = layoutGroup.Text.Substring(0, index - 1);
                        }

                        if (HighlightOptions.ShowCountsInTabs)
                        {
                            PropertyEditor propertyEditor = FindPropertyEditor(layoutControlItem);
                            object currentvalue = null;
                            if (propertyEditor != null && propertyEditor.Control != null)
                                currentvalue = propertyEditor.ControlValue;
                            else if (propertyEditor != null)
                                currentvalue = propertyEditor.MemberInfo.GetValue(propertyEditor.CurrentObject);

                            ICollection collection = currentvalue as ICollection;
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
                                isBold = currentvalue != null && !String.IsNullOrEmpty(currentvalue.ToString()) && HighlightOptions.BoldTabsWithCounts;
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
                    items = ((TabbedGroup)group).TabPages;
                else
                    items = ((LayoutGroup)group).Items;

                foreach (BaseLayoutItem item in items)
                {
                    if (item is LayoutItemContainer)
                        HighlightTabs((LayoutItemContainer)item);
                }
            }
            finally
            {
                group.EndUpdate();
            }
        }

        private PropertyEditor FindPropertyEditor(LayoutControlItem layoutItem)
        {
            if (View != null && layoutItem is XafLayoutControlItem)
                return ((WinLayoutManager)View.LayoutManager).FindViewItem(layoutItem) as PropertyEditor;

            return null;
        }
    }

    internal class PropertyEditorGroupHelper
    {
        public event EventHandler<LayoutGroupEventArgs> HighlightGroup;
        DetailView View = null;
        XafLayoutControl LayoutControl = null;
        Dictionary<PropertyEditor, BaseLayoutItem> editorMap = new Dictionary<PropertyEditor, BaseLayoutItem>();
        
        public void Initialize(DetailView view)
        {
            View = view;
            LayoutControl = View.Control as XafLayoutControl;

            Reset();
        }

        public void Reset()
        {
            ClearDictionary();

            if (LayoutControl != null)
                FindEditorsInTabs(LayoutControl.Root);
        }

        private void ClearDictionary()
        {
            foreach (PropertyEditor editor in editorMap.Keys)
            {
                editor.ValueStored -= Editor_ValueStored;

                if (editor.PropertyValue is IBindingList)
                    ((IBindingList)editor.PropertyValue).ListChanged -= PropertyEditorGroupHelper_ListChanged;
            }

            editorMap.Clear();
        }

        public void Destroy()
        {
            ClearDictionary();

            View = null;
            LayoutControl = null;
        }

        private void FindEditorsInTabs(LayoutItemContainer group)
        { 
            group.BeginUpdate();
            LayoutGroup layoutGroup = group as LayoutGroup;
            if ((layoutGroup != null) && layoutGroup.IsInTabbedGroup)
            {
                foreach (BaseLayoutItem item in layoutGroup.Items)
                {
                    if (item is XafLayoutControlItem)
                    {
                        PropertyEditor editor = ((WinLayoutManager)View.LayoutManager).FindViewItem((XafLayoutControlItem)item) as PropertyEditor;
                        if (editor != null)
                        {
                            editorMap.Add(editor, item);
                            editor.ValueStored += Editor_ValueStored;

                            if (editor.PropertyValue is IBindingList)
                                ((IBindingList)editor.PropertyValue).ListChanged += PropertyEditorGroupHelper_ListChanged;
                        }
                    }
                }
            }

            IEnumerable items = null;
            if (group is TabbedGroup)
                items = ((TabbedGroup)group).TabPages;
            else if (group is LayoutGroup)
                items = ((LayoutGroup)group).Items;

            if (items != null)
            {
                foreach (BaseLayoutItem child in items)
                {
                    if (child is LayoutItemContainer)
                    {
                        FindEditorsInTabs((LayoutItemContainer)child);
                    }
                }
            }

            group.EndUpdate();
        }

        private void PropertyEditorGroupHelper_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded || e.ListChangedType == ListChangedType.ItemDeleted)
                OnHighlightGroup(LayoutControl.Root);
        }

        private void Editor_ValueStored(object sender, EventArgs e)
        {
            PropertyEditor editor = (PropertyEditor)sender;
            BaseLayoutItem layoutitem = editorMap[editor];
            OnHighlightGroup(layoutitem.Parent);
        }

        private void OnHighlightGroup(LayoutGroup group)
        {
            if (group != null && HighlightGroup != null)
                HighlightGroup(this, new LayoutGroupEventArgs(group));
        }
    }

    public class LayoutGroupEventArgs : EventArgs
    {
        public LayoutGroup LayoutGroup { get; set; }

        public LayoutGroupEventArgs(LayoutGroup group) { LayoutGroup = group; }
    }
}
