using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TelenorDataServer.Merge
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths = new[]
            {
                @"C:\Users\hero_\Desktop\a.csv",
                @"C:\Users\hero_\Desktop\b.csv",
            };

            List<string> data = new List<string>();
            foreach (var path in paths)
            {
                string[] lines = File.ReadAllLines(path, Encoding.GetEncoding("ISO8859-1"));
                if (data.Count == 0)
                {
                    data.AddRange(lines);
                }
                else
                {
                    data.AddRange(lines.Skip(1));
                }
            }



            File.WriteAllLines(@"C:\Users\hero_\Desktop\invoice_details.20180614.csv", data);
            Console.WriteLine(true);
            Console.ReadKey();
        }
    }
}
