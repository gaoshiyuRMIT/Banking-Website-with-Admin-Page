using System;
using System.Collections.Generic;

namespace BankingAdmin.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Account = new HashSet<Account>();
            Login = new HashSet<Login>();
        }

        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Tfn { get; set; }

        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Login> Login { get; set; }
    }
}
