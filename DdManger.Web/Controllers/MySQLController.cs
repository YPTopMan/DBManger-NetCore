using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using DdManger.Web.Models;

namespace DdManger.Web.Controllers
{
    public class MySQLController : Controller
    {
        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Data Source=.;Initial Catalog=a;Persist Security Info=True;User ID=sa;pwd=sa", //必填
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.SystemTable
        });

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获得数据库
        /// </summary>
        /// <returns></returns>
        public IActionResult GetDataBase()
        {
            var list = new List<DbViewModel>();
            list = db.Ado.SqlQuery<DbViewModel>("SELECT name,filename FROM Master..SysDatabases ORDER BY Name")
            .ToList();
            return View(list);
        }

        /// <summary>
        /// 获得表
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        public IActionResult GetTables(string dbName)
        {
            //  --查询数据库中所有的表名及行数
            var sql = @" SELECT a.id,a.name, b.rows,c.value as description  FROM sysobjects AS a 
                        left JOIN sysindexes AS b ON a.id = b.id
                        left join sys.extended_properties c on a.id=c.major_id and minor_id=0 
                        WHERE (a.type = 'u') AND (b.indid IN (0, 1))  
                        ORDER BY a.name,b.rows DESC";

            var list = db.Ado.SqlQuery<TableViewModel>(sql).ToList();
            return View(list);
        }



    }
}