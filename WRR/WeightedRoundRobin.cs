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
        private int max { get; set; }
        private int gcd { get; set; }
        private int cw { get; set; }
        private int i { get; set; }

        public WeightedRoundRobin()
        {
            i = -1; // OrderBy weight -> index smallest weight sẽ được chọn sau cùng
            available = Users.Count();
            max = GetMaxWeight();
            gcd = GCD();
            cw = 0; // index có largest weight sẽ đc chọn trước
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
                            Weight = 4
                        },
                        new User()
                        {
                            Email = "B",
                            Weight = 2
                        },
                        new User()
                        {
                            Email = "C",
                            Weight = 0
                        },
                        new User()
                        {
                            Email = "D",
                            Weight = 8
                        }
                    }.OrderBy(a => a.Weight).ToList();
                }
                return users;
            }
        }

        public int GetMaxWeight()
        {
            return Users.OrderBy(n => n.Weight).Last().Weight;
        }

        public int GCD()
        {
            return Users.Select(n => n.Weight).ToList().Aggregate(GCD);
        }

        public int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public User GetUser()
        {
            while (true)
            {
                i = (i + 1) % available;
                //Console.WriteLine($"i = {i}");
                if (i == 0)
                {
                    cw = cw - gcd;
                    //Console.WriteLine($"cw = {cw}");
                    if (cw <= 0)
                    {
                        cw = max;
                        //Console.WriteLine($"cw max = {cw}");
                        if (cw == 0)
                            return null;
                    }
                }
                if (Users[i].Weight >= cw)
                {
                    return Users[i];
                }
            }
        }
    }
}