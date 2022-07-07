using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WRR
{
    public class WeightedRoundRobin
    {
        private List<User> users;
        private int available { get; set; }
        private double totalWeight { get; set; }
        private int totalTask { get; set; }
        private int i { get; set; }
        public WeightedRoundRobin()
        {
            available = Users.Count(); // 4
            totalWeight = Users.Sum(n => n.Weight);
            i = -1;

            foreach (User user in Users)
            {
                user.PercentWeight = (user.Weight / totalWeight) * 100;
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
                            Weight = 1
                        },
                        new User()
                        {
                            Email = "B",
                            Weight = 1
                        },
                        new User()
                        {
                            Email = "C",
                            Weight = 2
                        },
                        new User()
                        {
                            Email = "D",
                            Weight = 4
                        }
                    }.OrderBy(a => a.Weight).ToList();
                }
                return users.Where(n => n.Weight > 0).ToList();
            }
        }

        public User GetUser()
        {
            totalTask = 16;
            if (totalTask <= totalWeight)
            {
                while (true)
                {
                    i++;
                    if (i == available)
                    {
                        i = 0;
                    }
                    if (Users[i].Task < Users[i].Weight)
                    {
                        Users[i].Task++;
                        return Users[i];
                    }
                }
            }
            else
            {
                foreach (User user in Users)
                {
                    user.DynamicTasks = (totalTask * user.PercentWeight) / 100;
                    user.DynamicTasks = Math.Round(user.DynamicTasks, MidpointRounding.AwayFromZero);
                }

                while (true)
                {
                    i++;
                    if (i == available)
                    {
                        i = 0;
                    }
                    if (Users[i].Task < Users[i].DynamicTasks)
                    {
                        Users[i].Task++;
                        return Users[i];
                    }
                }
            }
        }
    }
}