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
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace LlamachantFramework.Module.Win.Controllers.General
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public class OpenObjectCaptionController : OpenObjectController, IModelExtender
    {
        public OpenObjectCaptionController()
        {
        }

        protected override void UpdateActionState(object objectToOpen)
        {
            base.UpdateActionState(objectToOpen);

            if ((Application.Model.Options as IModelOpenObjectOptions).ShowTypeNameInOpenObject)
            {
                string caption = Application.Model.ActionDesign.Actions["OpenObject"].Caption;
                string altcaption = CaptionHelper.GetLocalizedText("Texts", "OpenObjectWithCaption");
                OpenObjectAction.Caption = objectToOpen == null ? caption : String.Format(altcaption, CaptionHelper.GetClassCaption(objectToOpen.GetType().FullName));
            }
        }

        public void ExtendModelInterfaces(ModelInterfaceExtenders extenders)
        {
            extenders.Add<IModelOptions, IModelOpenObjectOptions>();
        }
    }

    public interface IModelOpenObjectOptions : IModelNode
    {
        [Category("Llamachant Framework")]
        bool ShowTypeNameInOpenObject { get; set; }
    }
}
