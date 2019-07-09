using System;
using System.IO;
using System.Text;

namespace TempLite.Services
{
    public class Log
    {
        public static string  logPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Temprecord\\TempLite\\log.txt";


        public static void Write(string logMessage)
        {
            //if (!File.Exists(logPath))
             //   File.Create(logPath);

            using (StreamWriter sw = File.AppendText(logPath))
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
