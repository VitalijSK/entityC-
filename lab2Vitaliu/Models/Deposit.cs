using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2Vitaliu.Models
{
    public class Deposit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan MinTime { get; set; }

        public double MinMoney { get; set; }

        public int IdCash { get; set; }

        public double Prosent { get; set; }

    }
}
