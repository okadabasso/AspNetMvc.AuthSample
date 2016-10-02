using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AuthSample2.Models
{
    public class User: AuthCore.AuthUser
    {
        [Column("type")]
        public string Type { get; set; } = "auth";

        [Column("memo")]
        [DisplayName("メモ")]
        public string Memo { get; set; }
    }
}