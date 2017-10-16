using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DdManger.Web.Models
{
    /// <summary>
    /// 表视图模型
    /// </summary>
    public class TableViewModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        [Display(Name = "表名")]
        public string Name { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [Display(Name = "行数")]
        public int Rows { get; set; }


        /// <summary>
        /// 表描述
        /// </summary>

        [Display(Name = "表描述")]
        public string d { get; set; }
    }
}
