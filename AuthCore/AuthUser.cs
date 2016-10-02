using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;
namespace AuthCore
{
    [Table("auth_users")]
    public class AuthUser : IUser
    {
        [Column("id")]
        [StringLength(255)]
        public string Id { get; set; }

        [Column("login_name")]
        [StringLength(255)]
        public string UserName { get; set; }

        [Column("password_hash")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Column("hash_key")]
        [StringLength(255)]
        public string HashKey { get; set; }

        [Column("login_retry_count")]
        public int LonginRetryCount { get; set; }


        [Column("lockout_end_date")]
        public DateTime? LockoutEndDate { get; set; }
    }
}
