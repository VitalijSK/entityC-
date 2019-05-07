using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2Vitaliu.Models
{
    public class MyContext : IdentityDbContext<User>
    {
        public DbSet<Role> roles { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Cash> cashes { get; set; }
        public DbSet<Deposit> deposits { get; set; }
        public DbSet<Client> clients { get; set; }

        public MyContext()
        {
            Database.EnsureCreated();
            //delete();
           // init();
        }
        private void delete()
        {
            if (cashes != null)
            {
                cashes.RemoveRange(cashes);
            }
            if (deposits != null)
            {
                deposits.RemoveRange(deposits);
            }
            if (clients != null)
            {
                clients.RemoveRange(clients);
            }
            this.SaveChanges();
        }
        private void init()
        {
            Cash dollar = new Cash();
            dollar.Name = "Dollar";
            Cash euro = new Cash();
            euro.Name = "Euro";
            Cash russuan = new Cash();
            russuan.Name = "Russua";

            cashes.Add(dollar);
            cashes.Add(euro);
            cashes.Add(russuan);
            this.SaveChanges();


            Deposit depositHomeset = new Deposit();
            depositHomeset.Name = "Home set";
            depositHomeset.MinTime = new TimeSpan(22,0,0);
            
            
            depositHomeset.MinMoney = 100;
            depositHomeset.IdCash = dollar.Id;
            depositHomeset.Prosent = 2;

            deposits.Add(depositHomeset);

            Deposit depositBorder = new Deposit();

            depositBorder.Name = "border";
            depositBorder.MinTime = new TimeSpan(12, 0, 0);
            
            depositBorder.MinMoney = 100;
            depositBorder.IdCash = euro.Id;
            depositBorder.Prosent = 1.5;

            deposits.Add(depositBorder);
            this.SaveChanges();

            Client vitalij = new Client();
            vitalij.FirstName = "Vitalij";
            vitalij.LastName = "Kovalevich";
            vitalij.Patronymic = "Sergeevich";
            vitalij.Telephone = "+37544123456";
            vitalij.DateDepositStart = new DateTime(2017, 6, 1);
            vitalij.DateDepositEnd = new DateTime(2018, 6, 1);
            vitalij.PassportDetails = "HB1231231";
            vitalij.CodeDeposit = depositHomeset.Id;
            vitalij.Summ = 1000;
            vitalij.SummEnd = vitalij.Summ + vitalij.Summ * depositHomeset.Prosent;
            vitalij.YearDeposit = depositHomeset.MinTime;
            vitalij.Worker = "Anton";
            clients.Add(vitalij);

            Client vlad = new Client();
            vlad.FirstName = "Vlad";
            vlad.LastName = "Kurbasku";
            vlad.Patronymic = "Sergeevich";
            vlad.Telephone = "+37544123456";
            vlad.DateDepositStart = new DateTime(2017, 1, 1);
            vlad.DateDepositEnd = new DateTime(2017, 6, 1);
            vlad.PassportDetails = "HB1231231";
            vlad.CodeDeposit = depositBorder.Id;
            vlad.Worker = "Gena";
            vlad.Summ = 1000;
            vlad.SummEnd = vlad.Summ + vlad.Summ * depositBorder.Prosent;
            vlad.YearDeposit = depositBorder.MinTime;
            clients.Add(vlad);

            this.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=hellooappdb1;Trusted_Connection=True;");
        }
    }
}
