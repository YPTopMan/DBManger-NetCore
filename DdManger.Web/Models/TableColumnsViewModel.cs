using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// 表名
        /// </summary>
        [Display(Name = "表名")]
        public string TableName { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        public string Explain { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        [Display(Name = "列名")]
        public string ColumnName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        public string ColumnType { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        [Display(Name = "是否为主键")]
        public string IsPk { get; set; }

        /// <summary>
        /// 是否标识列
        /// </summary>
        [Display(Name = "是否标识列")]
        public string IsIdentity { get; set; }

        /// <summary>
        /// 允许空
        /// </summary>
        [Display(Name = "允许空")]
        public string IsNullable { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        [Display(Name = "默认值")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 占用字节数
        /// </summary>
        [Display(Name = "占用字节数")]
        public string ByteLength { get; set; }

        /// <summary>
        /// 精度
        /// </summary>
        [Display(Name = "精度")]
        public string Precision { get; set; }

        /// <summary>
        /// 小数位数
        /// </summary>
        [Display(Name = "小数位数")]
        public string DecimalDigits { get; set; }

    }

}
