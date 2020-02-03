using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Banking.Attributes;

namespace Banking.Models
{
    public abstract class APerson
    {
        [Required, StringLength(50, ErrorMessage = "50 characters maximum.")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Only letters and spaces are allowed.")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "50 characters maximum.")]
        public string Address { get; set; }

        [StringLength(40, ErrorMessage = "40 characters maximum.")]
        public string City { get; set; }

        [RegularExpression(@"^[A-Z]{2,3}$",
            ErrorMessage = "Must be 2 or 3 upper case letters.")]
        [StringLength(3, MinimumLength = 2)]
        [AusState]
        public string State { get; set; }


        [Display(Name = "Post Code")]
        [RegularExpression(@"^\d{4}", ErrorMessage = "Must be 4 digits.")]
        [StringLength(4, MinimumLength = 4)]
        public string PostCode { get; set; }

        [RegularExpression(@"^\d{9}$", ErrorMessage = "Must be a 9-digit Australian phone number.")]
        [StringLength(9, MinimumLength = 9)]
        [Required]
        public string Phone { get; set; }
    }
}
