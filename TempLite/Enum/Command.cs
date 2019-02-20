using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TempLite
{
    public enum Command
    {
        WakeUp,
        SetRead,
        ReadLogger
    }
}
