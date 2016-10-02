using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace AuthSample2.Models
{
    public class ApplicationDbContext : AuthCore.AuthDbContext
    {
        public ApplicationDbContext() : base()
        {
        }

        public static new ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}