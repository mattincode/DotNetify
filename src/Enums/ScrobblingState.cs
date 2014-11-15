using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetify
{
    public enum ScrobblingState
    {
        UseGlobalSetting = 0,

        LocalEnabled = 1,

        LocalDisabled = 2,

        GlobalEnabled = 3,

        GlobalDisabled = 4
    }
}
