using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BankingLib.Models
{
    public partial class Customer : APerson
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Customer ID")]
        [Range(0, 9999, ErrorMessage = "No More Than 4 digits")]
        public int CustomerID { get; set; }


        
        [Display(Name = "TFN")]
        [StringLength(11, ErrorMessage = "Must be Tax File Number")]
        public string TFN { get; set; }

        public virtual List<Account> Accounts { get; set; }
    }

    public partial class Customer : APerson
    {
        
    }
}
