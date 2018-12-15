using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using DdManger.Web.Models;
using Microsoft.Extensions.Options;

namespace DdManger.Web.Controllers
{
    public class MySQLController : Controller
    {
        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = new ConfigHelper().Get<string>("mysql:ConnectionString"), //必填
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.SystemTable
        });


        public IActionResult Index(int isEdit = 0)
        {
            ViewBag.isEdit = isEdit;
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
            // select * from TABLES
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
            //  // SELECT * from  COLUMNS 
            var sql = @" show full columns from " + table;
            var list = db.Ado.SqlQuery<string>(sql).ToList();
            return View(list);
        }


        /// <summary>
        /// 修改表注释
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>      
        public int EditTableDescription(TableViewModel viewModel)
        {
            db.Ado.ExecuteCommand("alter table " + viewModel.Name +" comment @d ;", new { d = viewModel.Description });
            return 1;
        }

        /// <summary>
        /// 修改列注释
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditSingleTabCommit(TableViewModel viewModel)
        {
            var firstModel = EditTableDescription(viewModel);
            return Json(firstModel);
        }

        /// <summary>
        /// 修改列注释
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        private int EditColumns(string table, string column, string comment)
        {
            var sql = "alter table " + table + " modify column " + column + " int comment '" + comment + "'";
            var result = db.Ado.ExecuteCommand(sql);

            return result;
        }


        /// <summary>
        ///  保存列注释
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditListColCommit(List<TableColumnsViewModel> list)
        {
            var rowResult = 0;
            foreach (var item in list)
            {
                rowResult += EditColumns(item.TableName, item.ColumnName, item.Explain);
            }

            return Json(rowResult);
        }

        /// <summary>
        /// 保存列注释_单个
        /// </summary>
        public JsonResult EditSingleColCommit(TableColumnsViewModel item)
        {

            var result = EditColumns(item.TableName, item.ColumnName, item.Explain);

            return Json(result);
        }
    }
}
