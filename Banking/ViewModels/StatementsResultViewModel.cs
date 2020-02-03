using System;
using X.PagedList;

using Banking.Models;

namespace Banking.ViewModels
{
    public class StatementsResultViewModel
    {
        public IPagedList<Transaction> Transactions { get; set; }
        public Account Account { get; set; }
    }
}
