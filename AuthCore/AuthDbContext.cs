using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AuthCore
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext() : base("authdb")
        {

        }
        public static AuthDbContext Create()
        {
            return new AuthDbContext();
        }

        public DbSet<AuthUser> AuthUsers { get; set; }
    }
}
