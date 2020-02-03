using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Banking.Attributes
{
    public class AusStateAttribute : ValidationAttribute
    {
        public static readonly string[] states =
        {
            "NSW", "QLD", "SA", "TAS", "VIC", "WA", "ACT", "NT"
        };

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            string errMsg = "Must be a 2-or-3-letter Australian state, e.g. \"VIC\" for Victoria";
            string inputState = value as string;
            if (!string.IsNullOrEmpty(inputState))
                if (!Array.Exists(states, x => x == inputState))
                    return new ValidationResult(errMsg);
            return ValidationResult.Success;
        }
    }
}
