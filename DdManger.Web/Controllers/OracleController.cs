using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DdManger.Web.Models;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DdManger.Web.Controllers
{
    public class OracleController : Controller
    {
        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = new ConfigHelper().Get<string>("oracle:ConnectionString"), //必填
            DbType = DbType.Oracle,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.SystemTable
        });

        public IActionResult Index(int isEdit = 0)
        {
            ViewBag.isEdit = isEdit;
            return View();
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