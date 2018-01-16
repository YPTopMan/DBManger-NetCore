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
        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString= "server=192.168.0.188;uid=sa;pwd=sa;database=hms_dev",
          //  ConnectionString = "Data Source=.;Initial Catalog=a;Persist Security Info=True;User ID=sa;pwd=sa", //必填
            DbType = DbType.SqlServer,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.SystemTable
        });

        public DBController()
        {

        }

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

        /// <summary>
        /// 获得表
        /// </summary>
        /// <param name="dbName">数据库名</param>
        /// <returns></returns>
        public IActionResult GetColumns(string table) {
            var sql = @"SELECT d.name TableName,a.colorder 字段序号,a.name  ColumnName,
(case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end) IsIdentity,
(case when (SELECT count(*) FROM sysobjects  WHERE (name in (SELECT name FROM sysindexes
WHERE (id = a.id) AND (indid in  (SELECT indid FROM sysindexkeys  WHERE (id = a.id) AND (colid in  (SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name)))))))  AND (xtype = 'PK'))>0 then '√' else '' end) IsPk,
b.name  ColumnType,a.length  ByteLength ,
COLUMNPROPERTY(a.id,a.name,'PRECISION') as [Precision],  isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as  decimalDigits,
(case when a.isnullable=1 then '√'else '' end)  isnullable,isnull(e.text,'')  defaultValue,isnull(g.[value], ' ') AS [explain]
FROM  syscolumns a
left join systypes b on a.xtype=b.xusertype
inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties'
left join syscomments e on a.cdefault=e.id
left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id
left join sys.extended_properties f on d.id=f.class and f.minor_id=0
where b.name is not null and d.name=@tableName
order by a.id,a.colorder";

            var tcList = db.Ado.SqlQuery<TableColumnsViewModel>(sql, new SugarParameter("@tableName", table)).ToList();
            return View(tcList);
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
            var sql = @" SELECT a.id,a.name, b.rows,c.value as description  FROM sysobjects AS a 
                        left JOIN sysindexes AS b ON a.id = b.id
                        left join sys.extended_properties c on a.id=c.major_id and minor_id=0 
                        WHERE (a.type = 'u') AND (b.indid IN (0, 1))  and a.name=@tableName
                        ORDER BY a.name,b.rows DESC";

            var firstTable = db.Ado.SqlQuery<TableViewModel>(sql, new { tableName = table }).First();

            return View(firstTable);
        }

        /// <summary>
        /// 修改表注释
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditTableDescription(TableViewModel viewModel)
        {
            db.Ado.ExecuteCommand("EXECUTE sp_updateextendedproperty N'MS_Description', @d, N'user', N'dbo', N'table', @table, NULL, NULL", new { table = viewModel.Name, d = viewModel.Description });

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
            var sql = @"SELECT a.colorder 字段序号,a.name  ColumnName,
        (case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end) IsIdentity,
        (case when (SELECT count(*) FROM sysobjects  WHERE (name in (SELECT name FROM sysindexes
        WHERE (id = a.id) AND (indid in  (SELECT indid FROM sysindexkeys  WHERE (id = a.id) AND (colid in  (SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name)))))))  AND (xtype = 'PK'))>0 then '√' else '' end) IsPk,
        b.name  ColumnType,a.length  ByteLength ,
        COLUMNPROPERTY(a.id,a.name,'PRECISION') as [Precision],  isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as  decimalDigits,
        (case when a.isnullable=1 then '√'else '' end)  isnullable,isnull(e.text,'')  defaultValue,isnull(g.[value], ' ') AS [explain]
        FROM  syscolumns a
        left join systypes b on a.xtype=b.xusertype
        inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties'
        left join syscomments e on a.cdefault=e.id
        left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id
        left join sys.extended_properties f on d.id=f.class and f.minor_id=0
        where b.name is not null and d.name=@tableName and a.name=@columnName   
        order by a.id,a.colorder";

            var firstModel = db.Ado.SqlQuery<TableColumnsViewModel>(sql, new { tableName = table, columnName = column }).First();
            firstModel.TableName = table;
            return View(firstModel);
        }

        /// <summary>
        /// 修改列注释
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditTableCDescription(TableColumnsViewModel viewModel)
        {

            // sqlserver用语句给表的“字段”注释
            db.Ado.ExecuteCommand("EXECUTE sp_updateextendedproperty N'MS_Description', @d, N'user', N'dbo', N'table', @table, N'column',@cName", new { table = viewModel.TableName, cName = viewModel.ColumnName, d = viewModel.Explain });

            return View();
        }


        /// <summary>
        /// 所有未加注释的列
        /// </summary>
        /// <returns></returns>
        public IActionResult AllNotExp()
        {
            return View();
        }
    }
}
