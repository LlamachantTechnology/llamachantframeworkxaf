using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LlamachantFramework.Module.Interfaces
{
    public interface IAuditTrailUser
    {
        bool CanViewAuditTrail { get; }
    }
}
