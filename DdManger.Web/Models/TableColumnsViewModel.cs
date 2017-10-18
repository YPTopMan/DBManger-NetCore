using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DdManger.Web.Models
{

    /// <summary>
    /// 表列名
    /// </summary>
    public class TableColumnsViewModel
    {

        /// <summary>
        /// 说明
        /// </summary>
        public string Explain { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        public string IsPk { get; set; }

        /// <summary>
        /// 是否标识列
        /// </summary>
        public string IsIdentity { get; set; }

        /// <summary>
        /// 允许空
        /// </summary>
        public string IsNullable { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 占用字节数
        /// </summary>
        public string ByteLength { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        public string Precision { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        public string DecimalDigits { get; set; }


    }

}
