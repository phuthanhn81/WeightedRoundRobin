using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WRR
{
    class Program
    {
        static void Main(string[] args)
        {
            WeightedRoundRobin wrr = new WeightedRoundRobin();

            Thread t1 = new Thread(() => { One(wrr); });
            t1.Start();

            Thread t2 = new Thread(() => { Two(wrr); });
            //t2.Start();
        }

        static void Two(WeightedRoundRobin wrr)
        {
            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(7000);
                int index = new Random().Next(0, 3);

                Console.WriteLine(index);
                wrr.totalTask--;
                wrr.Users[index].Task--;
            }
        }

        static void One(WeightedRoundRobin wrr)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"csv_input.txt");
            string[] file = File.ReadAllLines(path);


            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();

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

                Thread.Sleep(500);
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

            File.WriteAllText(@"csv_output.txt", result.ToString());
        }
    }
}
