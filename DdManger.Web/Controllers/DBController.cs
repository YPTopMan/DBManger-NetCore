using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using DdManger.Web.Models;

namespace DdManger.Web.Controllers
{
    /// <summary>
    /// 数据库管理控制器
    /// </summary>
    public class DBController : Controller
    {
        SqlSugarClient db = null;

        //  SqlSugarClient db = new SqlSugarClient(null);

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

            list.Add(new DbViewModel { FileName = "fefwe", Name = "fewfwe" });
            list.Add(new DbViewModel { FileName = "fefwe22", Name = "fewfwe222" });
            //list = db.Ado.SqlQuery<DbViewModel>("SELECT name,filename FROM Master..SysDatabases ORDER BY Name")
            //    .ToList();

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
            //var sql = @"SELECT a.name, b.rows FROM sysobjects AS a
            //        INNER JOIN sysindexes AS b ON a.id = b.id
            //        WHERE(a.type = 'u') AND(b.indid IN(0, 1))
            //        ORDER BY a.name,b.rows DESC";

            //var list = db.Ado.SqlQuery<TableViewModel>(sql).ToList();

            var list = new List<TableViewModel>();
            list.Add(new TableViewModel { Name = "aa", Rows = 123, d = "few" });

            return View(list);
        }

        /// <summary>
        /// 获得所有被锁定的表
        /// </summary>
        /// <returns></returns>
        public IActionResult LockTables()
        {
            var sql = "select request_session_id  spid,OBJECT_NAME(resource_associated_entity_id) tableName   from   sys.dm_tran_locks where resource_type='OBJECT'";

            var result = db.Ado.SqlQuery<LockTableViewModel>(sql);

            return View();
        }

        /// <summary>
        /// 修改表注释
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditTableDescription(string table)
        {

            return View();
        }

        /// <summary>
        /// 修改表注释
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditTableDescription(string table, string d)
        {

            db.Ado.ExecuteCommand("EXECUTE sp_addextendedproperty N'MS_Description', @d, N'user', N'dbo', N'table', @table, NULL, NULL", new { table = "", d = "" });

            return View();
        }


        /// <summary>
        /// 修改列注释
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditTableCDescription(string table, string column)
        {
            return View();
        }

        /// <summary>
        /// 修改列注释
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditTableCDescription(string table, string column, string description)
        {            // sqlserver用语句给表的“字段”注释
            db.Ado.ExecuteCommand("EXECUTE sp_addextendedproperty N'MS_Description', @d, N'user', N'dbo', N'table', @table, N'column',@cName", new { table = "TestDBName", cName = "Id", d = "XXX" });

            return View();
        }

    }
}