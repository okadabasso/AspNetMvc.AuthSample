using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace AuthSample2.Models
{
    public class UserActivateViewModel
    {
        public string UserId { get; set;  }

        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }
    }
}