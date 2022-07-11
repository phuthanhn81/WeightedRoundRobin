using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WRR
{
    public class WeightedRoundRobin
    {
        private int loop;
        private List<User> users;
        public int available { get; set; }
        public double totalWeight { get; set; }
        public int totalTask { get; set; }
        public int i { get; set; }
        public WeightedRoundRobin()
        {
            available = Users.Count(); // 4
            totalWeight = Users.Sum(n => n.Weight);
            i = available;



            foreach (User user in Users)
            {
                //user.PercentWeight = (user.Weight / totalWeight) * 100;
            }
        }

        public List<User> Users
        {
            get
            {
                if (users == null)
                {
                    users = new List<User>()
                    {
                        new User()
                        {
                            Email = "A",
                            Weight = 2
                        },
                        new User()
                        {
                            Email = "B",
                            Weight = 4
                        },
                        new User()
                        {
                            Email = "C",
                            Weight = 1
                        },
                        new User()
                        {
                            Email = "D",
                            Weight = 1
                        }
                    }.OrderBy(a => a.Weight).ToList();
                }
                return users.Where(n => n.Weight > 0).ToList();
            }
        }

        public List<User> AllUsers
        {
            get
            {
                return users;
            }
        }

        public User GetUser()
        {
            if (totalTask % totalWeight == 0)
            {
                foreach (User user in Users)
                {
                    user.Task = 0;
                }
                i = available;
                totalTask = 0;
            }
            totalTask++;
            if (totalTask <= totalWeight)
            {
                while (true)
                {
                    loop++;
                    if (loop == 10)
                    {
                        Console.WriteLine("loop");
                    }
                    i--;
                    if (i == -1)
                    {
                        i = available - 1;
                    }
                    if (Users[i].Task < Users[i].Weight)
                    {
                        Users[i].Task++;
                        Users[i].AvailableTasks++;

                        loop = 0;
                        return Users[i];
                    }
                }
            }
            else
            {
                foreach (User user in Users)
                {
                    //user.AvailableTasks = (totalTask * user.PercentWeight) / 100;
                    user.AvailableTasks = Math.Round(user.AvailableTasks, MidpointRounding.AwayFromZero);
                }

                while (true)
                {
                    i++;
                    if (i == available)
                    {
                        i = 0;
                    }
                    if (Users[i].Task < Users[i].AvailableTasks)
                    {
                        Users[i].Task++;
                        return Users[i];
                    }
                }
            }
        }
    }
}