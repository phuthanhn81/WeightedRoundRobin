using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WRR
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"assignment_wrr_input.txt");
            string[] file = File.ReadAllLines(path);


            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            WeightedRoundRobin wrr = new WeightedRoundRobin();
            foreach (var item in file)
            {
                User data = wrr.GetUser();

                if (!dic.Keys.Contains(data.Email))
                {
                    dic.Add(data.Email, new List<string>() { item });
                }
                else
                {
                    dic[data.Email].Add(item);
                }
            }

            var result = new StringBuilder();

            foreach (var list in dic)
            {
                string line = list.Key + ":";
                foreach (var item in list.Value)
                {
                    line += " " + item + ",";
                }
                result.AppendLine(line.TrimEnd(','));
                result.AppendLine();
            }


            File.WriteAllText(@"csv.txt", result.ToString());

            Console.ReadLine();
        }
    }
}
