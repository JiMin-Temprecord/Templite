﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    interface IByteWriter
    {
        byte[] WriteBytes(byte[] sendMessage);
    }
}
