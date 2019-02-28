using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    class Hex
    {
        public Hex(string address, string reply)
        {
            Address = address;
            Reply = reply;
        }

        private string Address { get; set; }

        private string Reply { get; set; }

        public override string ToString()
        {
            return $"{Address}:{Reply}";
        }
    }
}
