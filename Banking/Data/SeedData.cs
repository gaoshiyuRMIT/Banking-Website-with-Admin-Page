using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

using BankingLib.Models;
using BankingLib.Data;

namespace Banking.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new BankingContext(serviceProvider.GetRequiredService<DbContextOptions<BankingContext>>());

            // Look for customers.
            if (context.Customer.Any())
                return; // DB has already been seeded.

            context.Customer.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    Name = "Matthew Bolger",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    PostCode = "3000",
                    Phone = "411111111"
                },
                new Customer
                {
                    CustomerID = 2200,
                    Name = "Rodney Cocker",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    PostCode = "3005",
                    Phone = "411111112"
                },
                new Customer
                {
                    CustomerID = 2300,
                    Name = "Shekhar Kalra",
                    Phone = "411111113"
                });

            context.Login.AddRange(
                new Login
                {
                    UserID = "12345678",
                    CustomerID = 2100,
                    PasswordHash = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2"
                },
                new Login
                {
                    UserID = "38074569",
                    CustomerID = 2200,
                    PasswordHash = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04"
                },
                new Login
                {
                    UserID = "17963428",
                    CustomerID = 2300,
                    PasswordHash = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE"
                });

            const string format = "dd/MM/yyyy hh:mm:ss tt";

            context.Account.AddRange(
                new Account
                {
                    AccountNumber = 4100,
                    AccountType = AccountType.Savings,
                    CustomerID = 2100,
                    Balance = 100,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = AccountType.Checking,
                    CustomerID = 2100,
                    Balance = 500,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = AccountType.Savings,
                    CustomerID = 2200,
                    Balance = 500.95m,
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)
                },
                new Account
                {
                    AccountNumber = 4300,
                    AccountType = AccountType.Checking,
                    CustomerID = 2300,
                    Balance = 1250.50m,
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });

            const string openingBalance = "Opening balance";
            context.Transaction.AddRange(
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4101,
                    Amount = 500,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4200,
                    Amount = 500.95m,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = TransactionType.Deposit,
                    AccountNumber = 4300,
                    Amount = 1250.50m,
                    Comment = "Opening balance",
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });
            Payee payee = new Payee
            {
                Name = "Jane Doe",
                Address = "789 Fake St",
                City = "Warrnambool",
                State = "VIC",
                PostCode = "3280",
                Phone = "411111111"
            };

            context.Payee.AddRange(payee);
            context.SaveChanges();

            context.BillPay.AddRange(
                new BillPay
                {
                    AccountNumber = 4100,
                    PayeeID = payee.PayeeID,
                    Amount = 10,
                    ScheduleDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null),
                    Period = BillPayPeriod.Monthly
                }
            );

            context.SaveChanges();
        }
    }
}
