using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DdManger.Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class DBLoginViewModel
    {
        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Display(Name = "登录名")]
        public string Name { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Display(Name = "登录密码")]
        public string Password { get; set; }
    }
}
