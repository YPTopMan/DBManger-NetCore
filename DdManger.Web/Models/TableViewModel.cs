using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int Rows { get; set; }


        /// <summary>
        /// 表描述
        /// </summary>

        public string d { get; set; }
    }
}
