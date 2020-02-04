using System;
using System.ComponentModel.DataAnnotations;
namespace BankingLib.Models

{
    public abstract class AModifyDate
    {
        private DateTime _modifyDate = DateTime.UtcNow;
        
        [Display(Name = "Modify Date")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ModifyDate { get => _modifyDate; set => _modifyDate = value; }

        [Display(Name = "Modify Date Local")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ModifyDateLocal => ModifyDate.ToLocalTime();
    }
}
