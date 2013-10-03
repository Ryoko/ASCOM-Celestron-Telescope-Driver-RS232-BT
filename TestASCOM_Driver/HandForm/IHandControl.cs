using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASCOM.CelestronAdvancedBlueTooth.HandForm
{
    interface IHandControl
    {
        void ShowForm(bool show);
        void SetForm(Telescope driver);
    }
}
