using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp
{
    internal class RuntimeLog
    {
        public const string DirectoryName = "temp";
        public const string FileName = "temp\\app.log";

        public static void Initialize()
        {
            if(!Directory.Exists("temp"))
            {
                Directory.CreateDirectory(DirectoryName);
            }

            File.WriteAllText(FileName, "-- APP RUN LOG --");
        }

        public static void WriteLog(string line)
        {
            //File.WriteAllText(FileName, $"\n[{DateTime.Now.ToString("hh:mm:ss : dd:MM:yyyy")}] {line}");
            if(File.Exists(FileName))
            {
                File.WriteAllText(FileName, $"{File.ReadAllText(FileName)}\n[{DateTime.Now.ToString("dd:MM:yyyy : hh:mm:ss")}] {line}");
            }
        }
    }
}
