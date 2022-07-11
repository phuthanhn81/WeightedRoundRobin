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
        static List<string> TasksNotAssigned { get; set; }

        static void Main(string[] args)
        {
            WeightedRoundRobin wrr = new WeightedRoundRobin();

            Thread t1 = new Thread(() => { One(wrr); });
            t1.Start();

            Thread t2 = new Thread(() => { Done(wrr); });
            t2.Start();

            Thread t3 = new Thread(() => { Off(wrr); });
            t3.Start();
        }

        static void Done(WeightedRoundRobin wrr)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(2000);
                int index = new Random().Next(0, wrr.available - 1);

                wrr.totalTask--;
                wrr.Users[index].Task--;

                int remove = new Random().Next(0, Convert.ToInt32(wrr.Users[index].AvailableTasks) - 1);
                Console.WriteLine($"{wrr.Users[index].Email} Done Task: {wrr.Users[index].DetailsTasks[remove]}");
                wrr.Users[index].DetailsTasks.RemoveAt(remove);
            }
        }

        static void One(WeightedRoundRobin wrr)
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"csv_input.txt");
            string[] file = File.ReadAllLines(path);

            foreach (var item in file)
            {
                User user = wrr.GetUser();
                user.DetailsTasks ??= new List<string>();
                user.DetailsTasksAll ??= new List<string>();

                user.DetailsTasks.Add(item);
                user.DetailsTasksAll.Add(item);

                Thread.Sleep(500);
            }

            var result = new StringBuilder();


            if (TasksNotAssigned != null)
            {
                foreach (string task in TasksNotAssigned)
                {
                    User user = wrr.GetUser();

                    user.DetailsTasks.Add(task);
                }
            }

            result.AppendLine("available");
            result.AppendLine();
            foreach (User user in wrr.Users)
            {
                string line = user.Email + ":";

                foreach (string task in user.DetailsTasks)
                {
                    line += " " + task + ",";
                }

                result.AppendLine(line.TrimEnd(','));
                result.AppendLine();
            }

            result.AppendLine("all done");
            result.AppendLine();

            foreach (User user in wrr.AllUsers)
            {
                string line = user.Email + ":";

                foreach (string task in user.DetailsTasks)
                {
                    line += " " + task + ",";
                }

                result.AppendLine(line.TrimEnd(','));
                result.AppendLine();
            }

            result.AppendLine("all");
            result.AppendLine();
            foreach (User user in wrr.AllUsers)
            {
                string line = user.Email + ":";

                foreach (string task in user.DetailsTasksAll)
                {
                    line += " " + task + ",";
                }

                result.AppendLine(line.TrimEnd(','));
                result.AppendLine();
            }

            File.WriteAllText(@"csv_output.txt", result.ToString());
        }

        static void Off(WeightedRoundRobin wrr)
        {
            Thread.Sleep(5000);
            int index = new Random().Next(0, wrr.available - 1);

            Console.WriteLine($"{wrr.Users[index].Email} Off");
            wrr.totalTask -= wrr.Users[index].Task;

            TasksNotAssigned ??= new List<string>();
            foreach (string task in wrr.Users[index].DetailsTasks)
            {
                Console.WriteLine(task);
                TasksNotAssigned.Add(task);
            }

            wrr.Users[index].Weight = 0;
            wrr.Users[index].Task = 0;
            wrr.Users[index].AvailableTasks = 0;

            wrr.available = wrr.Users.Count();
            wrr.totalWeight = wrr.Users.Sum(n => n.Weight);


        }
    }
}
