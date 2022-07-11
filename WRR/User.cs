using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WRR
{
    public class User
    {
        public string Email { get; set; }
        public double Weight { get; set; }
        public double Task { get; set; }
        public double AvailableTasks { get; set; }
        public List<string> DetailsTasks { get; set; }
        public List<string> DetailsTasksAll { get; set; }
        public string DetailsTasksString { get; set; }


        //public double PercentWeight { get; set; }
    }
}