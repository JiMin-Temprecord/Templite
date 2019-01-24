using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    class Hex
    {

        public string address { get; set; }

        public string reply { get; set; }

        public override string ToString()
        {
            return address + ":" + reply;
        }
    }
}
