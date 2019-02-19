using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempLite
{
    class PDF
    {

        public static void Preview()
        {
            Communication.ReadLogger();
            createPDF.getPDF();
        }

        public static void Email()
        { }

        public static void Download()
        { }


    }
}
