using System;
using System.ComponentModel.DataAnnotations;
using SimpleHashing;

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
        
        [Range(0, 3)]
        public int Attempts {get;set;} = 0;
        public DateTime? LockDateTime {get;set;}

        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }


        public bool IsLocked => LockDateTime != null;
        public bool LockExpired => IsLocked && LockDateTime.Value + TimeSpan.FromMinutes(1) <= DateTime.UtcNow;
        public void Lock()
        {
            LockDateTime = DateTime.UtcNow;
        }

        public void Unlock()
        {
            LockDateTime = null;
        }

        public bool Verify(string password)
        {
            return PBKDF2.Verify(PasswordHash, password);
        }
    }
}

