﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace BankingAdmin.Models
{
    public partial class Login
    {
        public string UserId { get; set; }
        public DateTime ModifyDate { get; set; }
        public string PasswordHash { get; set; }
        public int CustomerId { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }
    }
}