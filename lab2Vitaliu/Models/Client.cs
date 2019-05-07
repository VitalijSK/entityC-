using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2Vitaliu.Models
{
    public class Client
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public string Address { get; set; }

        public string Telephone { get; set; }

        public string PassportDetails { get; set; }

        public DateTime DateDepositStart { get; set; }

        public DateTime DateDepositEnd { get; set; }

        public int CodeDeposit { get; set; }

        public double Summ { get; set; }

        public double SummEnd { get; set; }

        public TimeSpan YearDeposit { get; set; }

        public string Worker { get; set; }

    }
}
