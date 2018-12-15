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
        /// 表描述(注释)
        /// </summary>
        [Display(Name = "表描述(注释)")]
        public string Description { get; set; }

        /// <summary>
        /// 类描述
        /// <para>该值，判断是否添加过注释</para>
        /// </summary>
        public string ClassDesc { get; set; }
    }
}
