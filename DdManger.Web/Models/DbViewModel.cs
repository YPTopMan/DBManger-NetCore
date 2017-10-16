using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace DdManger.Web.Models
{
    /// <summary>
    /// 数据库视图模型
    /// </summary>
    public class DbViewModel
    {
        /// <summary>
        /// 数据库名称
        /// </summary>    
        [Display(Name = "数据库名称")]
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [Display(Name = "数据库路径")]
        public string FileName { get; set; }
    }
}
