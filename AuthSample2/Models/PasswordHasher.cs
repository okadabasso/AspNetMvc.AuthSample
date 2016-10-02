using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace AuthSample2.Models
{
    public class PasswordHasher : Microsoft.AspNet.Identity.PasswordHasher
    {
        public override string HashPassword(string password)
        {
           
            return base.HashPassword(password);
        }
        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return base.VerifyHashedPassword(hashedPassword, providedPassword);
        }
    }
}