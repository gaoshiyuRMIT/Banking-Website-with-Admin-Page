using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using BankingAdmin.Models;

namespace BankingAdmin.Data
{
    public partial class BankingContext : DbContext
    {
        public BankingContext()
        {
        }

        public BankingContext(DbContextOptions<BankingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<BillPay> BillPay { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Login> Login { get; set; }
        public virtual DbSet<Payee> Payee { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountNumber);

                entity.HasIndex(e => e.CustomerId);

                entity.Property(e => e.AccountNumber).ValueGeneratedNever();

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Account)
                    .HasForeignKey(d => d.CustomerId);
            });

            modelBuilder.Entity<BillPay>(entity =>
            {
                entity.HasIndex(e => e.AccountNumber);

                entity.HasIndex(e => e.PayeeId);

                entity.Property(e => e.BillPayId).HasColumnName("BillPayID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.Property(e => e.PayeeId).HasColumnName("PayeeID");

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.BillPay)
                    .HasForeignKey(d => d.AccountNumber);

                entity.HasOne(d => d.Payee)
                    .WithMany(p => p.BillPay)
                    .HasForeignKey(d => d.PayeeId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.PostCode).HasMaxLength(4);

                entity.Property(e => e.State).HasMaxLength(3);

                entity.Property(e => e.Tfn)
                    .HasColumnName("TFN")
                    .HasMaxLength(11);
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasIndex(e => e.CustomerId);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(8);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Login)
                    .HasForeignKey(d => d.CustomerId);
            });

            modelBuilder.Entity<Payee>(entity =>
            {
                entity.Property(e => e.PayeeId).HasColumnName("PayeeID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.City).HasMaxLength(40);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.PostCode).HasMaxLength(4);

                entity.Property(e => e.State).HasMaxLength(3);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasIndex(e => e.AccountNumber);

                entity.HasIndex(e => e.DestAccountNumber);

                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.TransactionAccountNumberNavigation)
                    .HasForeignKey(d => d.AccountNumber);

                entity.HasOne(d => d.DestAccountNumberNavigation)
                    .WithMany(p => p.TransactionDestAccountNumberNavigation)
                    .HasForeignKey(d => d.DestAccountNumber);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
