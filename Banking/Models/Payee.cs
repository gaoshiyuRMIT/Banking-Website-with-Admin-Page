using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Banking.Models
{
    public class Payee : APerson
    {

        [Display(Name = "Payee ID")]
        [Range(0,9999, ErrorMessage = "No More Than 4 digits")]
        public int PayeeID { get; set; }

        [Display(Name = "Payee ID")]
        public string PayeeIDStr => PayeeID.ToString().PadLeft(4, '0');

        public override bool Equals(object obj)
        {
            Payee other = obj as Payee;
            return other != null && other.PayeeID == PayeeID;
        }

        public override int GetHashCode()
        {
            return PayeeID;
        }
    }
}
