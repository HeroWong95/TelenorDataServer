using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TelenorDataServer
{
    static class Logger
    {
        static StringBuilder builder = new StringBuilder();

        public static void Log(string text)
        {
            Console.WriteLine(text);
            builder.AppendLine(text);
        }

        public async static Task WriteLogAsync()
        {
            string name = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            const string dirName = "Logs";
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            await File.WriteAllTextAsync(Path.Combine(dirName, name), builder.ToString());
        }
    }
}
