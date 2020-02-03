using System;
using System.ComponentModel.DataAnnotations;
namespace Banking.Models

{
    public abstract class AModifyDate
    {
        [Display(Name = "Modify Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ModifyDate { get; set; }

        [Display(Name = "Modify Date Local")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ModifyDateLocal => ModifyDate.ToLocalTime();
    }
}
