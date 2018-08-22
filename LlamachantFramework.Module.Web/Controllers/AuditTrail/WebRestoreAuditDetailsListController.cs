using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using LlamachantFramework.Module.Controllers.AuditTrail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LlamachantFramework.Module.Web.Controllers.AuditTrail
{
    public class WebRestoreAuditDetailsListController : ViewController<ListView>
    {
        public WebRestoreAuditDetailsListController()
        {
            this.TargetObjectType = typeof(RestoreItemDetails);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
        }
    }
}
