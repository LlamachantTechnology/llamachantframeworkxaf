using DevExpress.ExpressApp.Win.Utils;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.Win.Utils
{
    public class CustomSplashScreen : DXSplashScreen
    {
        public CustomSplashScreen(Type xtrasplashformtype) : base(xtrasplashformtype) { }

        public override void UpdateSplash(string caption, string description, params object[] additionalParams)
        {
            base.UpdateSplash(caption, description, additionalParams);

            if (SplashScreenManager.Default != null)
                SplashScreenManager.Default.SendCommand(SplashScreenUpdate.Blank, String.Format("{0}|{1}", caption, description));
        }

        public enum SplashScreenUpdate { Blank }
    }
}
