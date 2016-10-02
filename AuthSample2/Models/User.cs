using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace AuthSample2.Models
{
    public class User: IUser
    {
        public string Id { get; set; }

        [DisplayName("ユーザ名")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("パスワード")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("メモ")]
        public string Memo { get; set; }
    }
}