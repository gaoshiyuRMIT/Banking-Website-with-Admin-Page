using System;
using System.ComponentModel.DataAnnotations;

namespace Banking.Models
{
    public class Login : AModifyDate
    {


        [Key]
        [Display(Name = "User ID")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "No More Than 8 digits")]
        [RegularExpression(@"^\d{8}", ErrorMessage = "Only Numbers Allowed")]
        public string UserID { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }
}

