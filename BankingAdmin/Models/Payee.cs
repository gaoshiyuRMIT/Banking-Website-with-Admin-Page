using System;
using System.Collections.Generic;

namespace BankingAdmin.Models
{
    public partial class Payee
    {
        public Payee()
        {
            BillPay = new HashSet<BillPay>();
        }

        public int PayeeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<BillPay> BillPay { get; set; }
    }
}
