using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Common
{
    public interface IAppliance
    {
        string ApplianceName { get; }
        string ApplianceType { get; } 
        string IconImage { get; }
    }
}
