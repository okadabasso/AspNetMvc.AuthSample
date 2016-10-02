using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AuthSample2.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "現在のパスワード")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} の長さは {2} 文字以上である必要があります。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワード")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワードの確認入力")]
        [Compare("NewPassword", ErrorMessage = "新しいパスワードと確認のパスワードが一致しません。")]
        public string ConfirmPassword { get; set; }
    }
}