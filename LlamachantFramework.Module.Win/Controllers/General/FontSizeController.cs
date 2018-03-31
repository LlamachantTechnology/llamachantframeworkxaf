using System;
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
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using DevExpress.XtraBars;

namespace LlamachantFramework.Module.Win.Controllers.General
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FontSizeController : WindowController, IModelExtender
    {
        public FontSizeController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetWindowType = WindowType.Main;
        }

        internal IModelFontSizeOptions FontOptions { get { return Application == null ? null :  Application.Model.Options as IModelFontSizeOptions; } }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

            actionChooseFontSize.Active[this.Name] = FontOptions.AllowFontSizeSelection;

            PopulateFontSizes();
            this.Window.TemplateChanged += Window_TemplateChanged;
        }

        private void Window_TemplateChanged(object sender, EventArgs e)
        {
            SetFontSize();
        }

        protected override void OnDeactivated()
        {
            this.Window.TemplateChanged -= Window_TemplateChanged;
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void PopulateFontSizes()
        {
            actionChooseFontSize.Items.Clear();

            IModelFontSizeOptions options = Application.Model.Options as IModelFontSizeOptions;
            foreach (IModelFontSizeOption size in options.FontSizes.OrderBy(x=>x.Index).ThenBy(x=>x.FontSize).ThenBy(x=>x.Caption))
            {
                actionChooseFontSize.Items.Add(new ChoiceActionItem(size.Caption, size.FontSize));
            }
        }

        private void actionChooseFontSize_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            FontOptions.FontSizes.SelectedFontSize = FontOptions.FontSizes.Where(x => x.Caption == e.SelectedChoiceActionItem.Caption).First();
            SetFontSize();
        }

        private void SetFontSize()
        {
            if (FontOptions.AllowFontSizeSelection && FontOptions.FontSizes.SelectedFontSize != null)
            {
                AppearanceObject.DefaultFont = new Font(AppearanceObject.DefaultFont.FontFamily, (float)FontOptions.FontSizes.SelectedFontSize.FontSize);
                if (Frame.Template is IBarManagerHolder)
                {
                    BarManager manager = ((IBarManagerHolder)Frame.Template).BarManager;
                    if (manager.Controller == null)
                        manager.Controller = new BarAndDockingController();

                    manager.Controller.AppearancesBar.MainMenu.Font = AppearanceObject.DefaultFont;
                    manager.Controller.AppearancesBar.ItemsFont = AppearanceObject.DefaultFont;
                }

                foreach (ChoiceActionItem item in actionChooseFontSize.Items)
                {
                    item.ImageName = FontOptions.FontSizes.SelectedFontSize.Caption == item.Caption ? "BO_Validation" : null;
                }
            }
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<DevExpress.ExpressApp.Model.IModelOptions, IModelFontSizeOptions>();
        }
    }

    public interface IModelFontSizeOptions : IModelNode
    {
        [Category("Llamachant Framework")]
        bool AllowFontSizeSelection { get; set; }
        [Category("Llamachant Framework")]
        IModelFontSizeOptionList FontSizes { get; }
    }

    [ModelNodesGenerator(typeof(FontSizeOptionsNodesGenerator))]
    public interface IModelFontSizeOptionList : IModelNode, IModelList<IModelFontSizeOption>
    {
        [Category("Llamachant Framework")]
        [DataSourceProperty("this")]
        IModelFontSizeOption SelectedFontSize { get; set; }
    }

    [DisplayProperty("Caption")]
    public interface IModelFontSizeOption : IModelNode
    {
        string Caption { get; set; }
        double FontSize { get; set; }
    }

    public class FontSizeOptionsNodesGenerator : ModelNodesGeneratorBase
    {
        protected override void GenerateNodesCore(ModelNode node)
        {
            AddFontSizeOption(node, "Tiny", 6);
            AddFontSizeOption(node, "Small", 7.5);
            AddFontSizeOption(node, "Normal", 8.25);
            AddFontSizeOption(node, "Large", 10);
            AddFontSizeOption(node, "Extra Large", 12);
        }

        private void AddFontSizeOption(ModelNode node, string caption, double size)
        {
            IModelFontSizeOption option = node.AddNode<IModelFontSizeOption>(caption);
            option.Caption = caption;
            option.FontSize = size;
        }

    }
}
