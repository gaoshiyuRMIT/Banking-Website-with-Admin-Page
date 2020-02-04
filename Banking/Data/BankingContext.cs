using System;
using Microsoft.EntityFrameworkCore;

using BankingLib.Models;

namespace Banking.Data
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options)
            : base(options)
        {
        }

        public DbSet<Login> Login { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<BillPay> BillPay { get; set; }
        public DbSet<Payee> Payee { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>().HasOne<Account>(x => x.Account)
                .WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);
            modelBuilder.Entity<Transaction>().HasOne<Account>(x => x.DestAccount)
                .WithMany().HasForeignKey(x => x.DestAccountNumber)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
