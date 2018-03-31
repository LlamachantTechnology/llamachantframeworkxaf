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
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.LookAndFeel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Skins;

namespace LlamachantFramework.Module.Win.Controllers.General
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SvgSkinController : WindowController, IModelExtender
    {
        public SvgSkinController()
        {
            InitializeComponent();
            this.TargetWindowType = WindowType.Main;
        }

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            ChooseSkinController chooseSkinController = Frame.GetController<ChooseSkinController>();
            if (chooseSkinController != null)
                chooseSkinController.ChooseSkinAction.ExecuteCompleted += ChooseSkinAction_ExecuteCompleted;
        }

        private void ChooseSkinAction_ExecuteCompleted(object sender, ActionBaseEventArgs e)
        {
            RestorePalette(Application);
            UpdateActionActivity(actionThemeOptions);
        }

        public static void RestorePalette(XafApplication application)
        {
            var model = (IModelApplicationOptionsSkinSvg)application.Model.Options;
            if (model.Palette == null) return;
            var skin = CommonSkins.GetSkin(UserLookAndFeel.Default);
            DevExpress.Utils.Svg.SvgPalette palette = skin.CustomSvgPalettes[model.Palette];
            if (palette != null)
            {
                skin.SvgPalettes[Skin.DefaultSkinPaletteName].SetCustomPalette(palette);
                LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
            }
        }
        public static void SavePalette(XafApplication application)
        {
            var model = (IModelApplicationOptionsSkinSvg)application.Model.Options;
            var commonSkin = CommonSkins.GetSkin(UserLookAndFeel.Default);
            DevExpress.Utils.Svg.SvgPalette customPallete = commonSkin.SvgPalettes[Skin.DefaultSkinPaletteName].CustomPalette;
            var name = commonSkin.CustomSvgPalettes.FirstOrDefault(x => x.Value == customPallete).Key?.Name;
            model.Palette = name;
        }

        protected override void UpdateActionActivity(ActionBase action)
        {
            base.UpdateActionActivity(action);
            var isSvgSkin = UserLookAndFeel.Default.ActiveStyle == ActiveLookAndFeelStyle.Skin
                && UserLookAndFeel.Default.ActiveSkinName == SkinStyle.Bezier;
            action.Active["Svg"] = isSvgSkin;
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelApplicationOptionsSkin, IModelApplicationOptionsSkinSvg>();
        }


        private void actionThemeOptions_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var form = ((WinWindow)Window).Form;
            using (var dialog = new DevExpress.Customization.SvgSkinPaletteSelector(form))
            {
                dialog.ShowDialog();
                SavePalette(Application);
            }
        }
    }

    public interface IModelApplicationOptionsSkinSvg
    {
        [Category("Llamachant Framework")]
        [Browsable(false)]
        string Palette { get; set; }
    }
}
