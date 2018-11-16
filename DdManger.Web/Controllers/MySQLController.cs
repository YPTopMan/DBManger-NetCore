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
            var databases = db.Ado.SqlQuery<string>("show databases;").ToList();         
            return View(databases);
        }

        /// <summary>
        /// 获得表
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        public IActionResult GetTables(string dbName)
        {
            var sql = @"show tables from " + dbName;
            var list = db.Ado.SqlQuery<string>(sql).ToList();
            return View(list);
        }

        /// <summary>
        /// 获得表中所有列
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        public IActionResult GetColumns(string table)
        {            
            var sql = @" show columns from " + table;
            var list = db.Ado.SqlQuery<string>(sql).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
