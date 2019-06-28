using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempLite.Services
{
    public class Log
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory + "Email\\";

        public static void Write(string logMessage)
        {
            using (StreamWriter sw = File.AppendText("log.txt"))
            {
                sw.Write($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}");
                sw.WriteLine($" : {logMessage}");
                sw.Close();
            }
        }

        public static string Read(string filename)
        {
            StringBuilder logString = new StringBuilder();
            string line;
            using (StreamReader sr = File.OpenText(filename))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    logString.Append(line + "\r\n");
                }
            }

            return logString.ToString();
        }
    }
}
