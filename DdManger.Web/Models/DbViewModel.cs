using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string FileName { get; set; }
    }
}
