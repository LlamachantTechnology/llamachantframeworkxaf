using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using LlamachantFramework.Module.Interfaces;
using LlamachantFramework.Module.ModelGenerators;
using LlamachantFramework.Module.Controllers.General;
using LlamachantFramework.Module.Interfaces.Model;
using LlamachantFramework.Module.Controllers.AuditTrail;
using DevExpress.ExpressApp.AuditTrail;

namespace LlamachantFramework.Module {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class LlamachantFrameworkModule : ModuleBase {
        public LlamachantFrameworkModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.

            application.CustomProcessShortcut += Application_CustomProcessShortcut;
            application.SetupComplete += Application_SetupComplete;
        }

        private void Application_SetupComplete(object sender, EventArgs e)
        {
            ViewAuditTrailController.AuditTrailEnabled = this.Application.Modules.FindModule<AuditTrailModule>() != null;
        }

        private void Application_CustomProcessShortcut(object sender, CustomProcessShortcutEventArgs e)
        {
            IModelListView view = Application.Model.Views[e.Shortcut.ViewId] as IModelListView;
            if (view != null)
            {
                Type t = XafTypesInfo.Instance.FindTypeInfo(view.AsObjectView.ModelClass.Name).Type;
                if (t.GetInterface(typeof(ISingletonBO).FullName) != null)
                {
                    IObjectSpace space = Application.CreateObjectSpace(t);
                    object obj = space.FindObject(t, null, true);
                    if (obj == null)
                        obj = space.CreateObject(t);

                    e.View = Application.CreateDetailView(space, obj);                    
                    e.Handled = true;
                }
            }
        }

        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelOptions, IModelHighlightOptions>();
        }
        public override void AddGeneratorUpdaters(ModelNodesGeneratorUpdaters updaters)
        {
            base.AddGeneratorUpdaters(updaters);
            updaters.Add(new ISingletonBOModelUpdater());
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
