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
        static string TasksNotAssigned { get; set; }

        static void Main(string[] args)
        {
            WeightedRoundRobin wrr = new WeightedRoundRobin();

            Thread t1 = new Thread(() => { One(wrr); });
            t1.Start();

            Thread t2 = new Thread(() => { Two(wrr); });
            t2.Start();

            Thread t3 = new Thread(() => { Three(wrr); });
            t3.Start();
        }

        static void Two(WeightedRoundRobin wrr)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(2000);
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
                User user = wrr.GetUser();

                user.DetailsTasks += " " + item + ",";

                Thread.Sleep(500);
            }

            var result = new StringBuilder();


            if (TasksNotAssigned != null)
            {
                foreach (string task in TasksNotAssigned.Split(','))
                {
                    User user = wrr.GetUser();

                    user.DetailsTasks += " " + task + ",";
                }
            }

            foreach (User user in wrr.Users)
            {
                string line = user.Email + ":";

                line += " " + user.DetailsTasks + ",";

                result.AppendLine(line.TrimEnd(','));
                result.AppendLine();
            }

            foreach (User user in wrr.AllUsers)
            {
                string line = user.Email + ":";

                line += " " + user.DetailsTasks + ",";

                result.AppendLine(line.TrimEnd(','));
                result.AppendLine();
            }

            File.WriteAllText(@"csv_output.txt", result.ToString());
        }

        static void Three(WeightedRoundRobin wrr)
        {
            Thread.Sleep(5000);
            int index = new Random().Next(0, 3);
            wrr.Users[index].Weight = 0;
            wrr.Users[index].Task = 0;
            wrr.Users[index].AvailableTasks = 0;

            wrr.available = wrr.Users.Count();
            wrr.totalWeight = wrr.Users.Sum(n => n.Weight);

            TasksNotAssigned = wrr.Users[index].DetailsTasks;
        }
    }
}
