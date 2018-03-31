using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DevExpress.ExpressApp.TreeListEditors.Win;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Utils.Menu;
using DevExpress.XtraTreeList;

namespace LlamachantFramework.Module.Win.Controllers.General
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class SummaryPanelOptionsController : ViewController<ListView>, IModelExtender
    {
        public SummaryPanelOptionsController()
        {
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }

        public bool AllowSummaryPanel { get { return (this.View.Model as IModelSummaryPanelListViewOptions).HasValue("AllowSummaryPanelMenuItem") ? (this.View.Model as IModelSummaryPanelListViewOptions).AllowSummaryPanelMenuItem : (Application.Model.Options as IModelSummaryPanelOptions).AllowSummaryPanelMenuItem; } }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            if (AllowSummaryPanel)
            {
                ISupportFooter editor = this.View.Editor as ISupportFooter;

                if (editor != null)
                {
                    if (this.View.Editor is GridListEditor)
                        (this.View.Editor as GridListEditor).GridView.PopupMenuShowing += GridView_PopupMenuShowing;
                    else if (this.View.Editor is TreeListEditor)
                        (this.View.Editor as TreeListEditor).TreeList.PopupMenuShowing += TreeList_PopupMenuShowing;
                }
            }
        }

        protected override void OnDeactivated()
        {
            if (AllowSummaryPanel)
            {
                if (this.View.Editor is GridListEditor && (this.View.Editor as GridListEditor).GridView != null)
                    (this.View.Editor as GridListEditor).GridView.PopupMenuShowing -= GridView_PopupMenuShowing;
                else if (this.View.Editor is TreeListEditor && (this.View.Editor as TreeListEditor).TreeList != null)
                    (this.View.Editor as TreeListEditor).TreeList.PopupMenuShowing -= TreeList_PopupMenuShowing;
            }

            base.OnDeactivated();
        }

        private bool IsFooterVisible
        {
            get { return (this.View.Editor as ISupportFooter).IsFooterVisible; }
            set { (this.View.Editor as ISupportFooter).IsFooterVisible = value; }
        }

        void TreeList_PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
        {
            TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(e.Point);
            if (hitInfo.HitInfoType == HitInfoType.Column || hitInfo.HitInfoType == HitInfoType.SummaryFooter)
                AddMenuItem(e.Menu);
        }

        void GridView_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
        {
            if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Column)
                AddMenuItem(e.Menu);
        }

        private void AddMenuItem(DXPopupMenu menu)
        {
            string caption = IsFooterVisible ? CaptionHelper.GetLocalizedText("Texts", "SummaryPanelHide") : CaptionHelper.GetLocalizedText("Texts", "SummaryPanelShow");
            menu.Items.Add(new DevExpress.Utils.Menu.DXMenuItem(caption, OnMenuItemClick));
        }

        protected void OnMenuItemClick(object sender, EventArgs e)
        {
            IsFooterVisible = !IsFooterVisible;
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelOpenObjectOptions, IModelSummaryPanelOptions>();
            extenders.Add<IModelListView, IModelSummaryPanelListViewOptions>();
        }
    }

    public interface IModelSummaryPanelOptions : IModelNode
    {
        [Category("Llamachant Framework")]
        [DefaultValue(false)]
        bool AllowSummaryPanelMenuItem { get; set; }
    }

    public interface IModelSummaryPanelListViewOptions : IModelNode
    {
        [Category("Llamachant Framework")]
        [DefaultValue(false)]
        bool AllowSummaryPanelMenuItem { get; set; }
    }
}
