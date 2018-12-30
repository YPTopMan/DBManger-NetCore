using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DdManger.Web.Models
{
    public class MySqlLockTableViewModel
    {
        /// <summary>
        ///     
        /// </summary>
        public string Database { get; set; }

        public string Table { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public string In_Use { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name_Locked { get; set; }

    }
}
