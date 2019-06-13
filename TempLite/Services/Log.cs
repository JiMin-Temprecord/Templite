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
        public static void WritetoLog(string logMessage)
        {
            using (StreamWriter sw = File.AppendText("log.txt"))
            {
                sw.Write($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}");
                sw.WriteLine($" : {logMessage}");
                sw.Close();
            }
        }

        public static void ReadFromlog(string filename)
        {
            string line;
            using (StreamReader sr = File.OpenText(filename))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        public static void AddEmail(string textFile, string Email)
        {
            using (StreamWriter sw = File.AppendText(textFile))
            {
                sw.WriteLine(Email);
                sw.Close();
            }
        }

        public static bool CheckEmail(string textFile, string Email)
        {
            string line;
            using (StreamReader sr = File.OpenText(textFile))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == Email)
                        return true;
                }

                return false;
            }
        }
    }
}
