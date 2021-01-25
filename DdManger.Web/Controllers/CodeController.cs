using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdManger.Web.Models;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace DdManger.Web.Controllers
{
    public class CodeController : Controller
    {

        SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            //ConnectionString = new ConfigHelper().Get<string>("sqlserver:ConnectionString"),       
            //DbType = DbType.SqlServer,
            ConnectionString = new ConfigHelper().Get<string>("mysql:ConnectionString"),
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.SystemTable
        });

        public string diskPath { get; set; }

        public string EnumStr = @"using System;
using System.Collections.Generic;
using System.Text;

namespace JytPlatformServer.DtoModels.Common.Enums
{";

        public string EnumName = "";

        public CodeController()
        {
            diskPath = @"D:\IntelligentCode\" + DateTime.Now.ToString("yyyyMMdd");

            if (!Directory.Exists(diskPath))
            {
                Directory.CreateDirectory(diskPath + @"\R");
                Directory.CreateDirectory(diskPath + @"\V");
                Directory.CreateDirectory(diskPath + @"\C");
                Directory.CreateDirectory(diskPath + @"\IS");
                Directory.CreateDirectory(diskPath + @"\S");
                Directory.CreateDirectory(diskPath + @"\E");
                Directory.CreateDirectory(diskPath + @"\M");
            }
        }

        /// <summary>
        /// http://localhost:51367/IntelligentCode?models=案例库,FailureCase*2|案例库解决方法,FailureSolution*2|案例库分析,FailureAnalysise*2
        ///  http://localhost:51367/IntelligentCode?models=报修单原因分析,FaultPhenomenonAnalysi*2|维修单解决方案,FaultPhenomenonSolution*2
        ///  http://localhost:51367/IntelligentCode?models=申请案例库,ApplicationFailureCase*2|申请案例库解决方法,ApplicationFailureSolution*2|申请案例库分析,ApplicationFailureAnalysise*2
        /// http://localhost:51367/IntelligentCode?models=设备用途,EquipmentUse*2|设备工作方式,EquipmentWorkMode*2
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public IActionResult Index(string models)
        {
            if (!string.IsNullOrEmpty(models))
            {
                var modelArray = models.Split('|');

                for (int i = 0; i < modelArray.Length; i++)
                {
                    var currentModel = modelArray[i];
                    if (!string.IsNullOrEmpty(currentModel))
                    {
                        var currentModels = currentModel.Split(',');

                        var currentNamezh = currentModels.FirstOrDefault();
                        var modelName = currentModels.LastOrDefault();
                        if (!string.IsNullOrEmpty(modelName))
                        {
                            var modelLength = modelName.Split('*');

                            // 重新赋值
                            modelName = modelLength.FirstOrDefault();
                            getRepository(currentNamezh, modelName, modelName);

                            if (modelLength.Length > 1)
                            {
                                getIServer(currentNamezh, modelName);
                                getServer(currentNamezh, modelName);
                                getController(currentNamezh, modelName);
                                getViewModel(currentNamezh, modelName);
                                //  getModel(modelName, currentNamezh);
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(EnumName))
                    CreateFile(diskPath + @"\E\" + EnumName, EnumStr);
            }
            return Json("成功");
        }

        /// <summary>
        /// http://localhost:51367/Code/Plus?models=%E8%80%83%E5%8B%A4%E5%B9%B4%E5%BA%A6|AppraisePlanYear|AppraisePlan|AppraisePlan-AppraisePersonal|file,message,Employee,guid[],Approval
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public IActionResult Plus(string models)
        {
            if (!string.IsNullOrEmpty(models))
            {

                //    

                var modelArray = models.Split('|');
                var sub1Str = modelArray[2];
                var sub1List = sub1Str.Split(',').Select(t => t).ToList();

                var sub2Str = modelArray[3];
                var sub2List = new List<SubItem>();
                sub2Str.Split(',').Select(t => t).ToList().ForEach(t =>
                {

                    var subItemList = t.Split('-');
                    if (subItemList.Any())
                    {
                        sub2List.Add(new SubItem
                        {
                            Parent = subItemList[0],
                            Item = subItemList[1]
                        });
                    }
                });

                //  var a = 0;

                getServerPlus(modelArray.FirstOrDefault(), modelArray[1], sub1List, sub2List, modelArray[4]);

                //for (int i = 0; i < modelArray.Length; i++)
                //{
                //    var currentModel = modelArray[i];
                //    if (!string.IsNullOrEmpty(currentModel))
                //    {
                //        var currentModels = currentModel.Split(',');

                //        var currentNamezh = currentModels.FirstOrDefault();
                //        var modelName = currentModels.LastOrDefault();
                //        if (!string.IsNullOrEmpty(modelName))
                //        {
                //            var modelLength = modelName.Split('*');

                //            // 重新赋值
                //            modelName = modelLength.FirstOrDefault();
                //            getRepository(currentNamezh, modelName, modelName);

                //            if (modelLength.Length > 1)
                //            {
                //                getIServer(currentNamezh, modelName);
                //                getServer(currentNamezh, modelName);
                //                getController(currentNamezh, modelName);
                //                getViewModel(currentNamezh, modelName);
                //                //  getModel(modelName, currentNamezh);
                //            }
                //        }
                //    }
                //}

                //if (!string.IsNullOrEmpty(EnumName))
                //    CreateFile(diskPath + @"\Plus\" + EnumName, EnumStr);
            }
            return Json("成功");
        }



        public void CreateFile(string path, string content)
        {
            //if (!System.IO.File.Exists(path))
            //{
            //    System.IO.File.Create(path);
            //}
            System.IO.File.WriteAllText(path, content);
        }

        public void getRepository(string name, string modelName, string entityName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"using JytPlatformServer.DbModels.BusinessModels;
using System;
using System.Collections.Generic;
using System.Text;
using JytPlatformServer.DbModels.BusinessModels;

namespace JytPlatformServer.DbRepositories.BusinessRepositories
{
    /// <summary>
    ");
            sb.Append("/// " + name + "仓储类");
            sb.Append(@"  
    /// </summary>   
    public class " + modelName + "Repository : BaseBusinessRepository<" + entityName + ">");
            sb.Append(@" 
    {        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name=""context""></param>
        ");
            sb.Append("public " + modelName + "Repository(JytDbPlatformBusinessContext context) : base(context)");
            sb.Append(@"
        {
        }
    }
}
");            //return sb.ToString();    

            CreateFile(diskPath + @"\R\" + modelName + "Repository.cs", sb.ToString());
        }


        public void getController(string name, string modelName)
        {
            var lowerName = modelName.ToLower();

            string str = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JYT.JytDtoModels.WebApiModels;
using JytPlatformServer.Business.IServices;
using JytPlatformServer.DtoModels.BusinessDtoModels;
using JytPlatformServer.WebAPI.Common.BusinessCommon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JytPlatformServer.WebAPI.Controllers.BusinessControllers
{
    /// <summary>
    /// " + name + @"控制器
    /// </summary>
    public class " + modelName + @"Controller : BaseBusinessJytPlatformServerCommonController
    {
        readonly I" + modelName + @"Service   _" + lowerName + @"Service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name=" + lowerName + @"Service"" ></param>
        public " + modelName + @"Controller(I" + modelName + @"Service " + lowerName + @"Service)
        {
            _" + lowerName + @"Service = " + lowerName + @"Service;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""model"" ></param>
        /// <returns></returns>     
        [HttpPost, Route(nameof(Add))]
        public async Task<HttpMessageModel> Add(" + modelName + @"RequestDtoModel model)
        {
            return await _" + lowerName + @"Service.AddAsync(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids"" > 业务列表Id</param>
        /// <returns></returns>       
        [HttpPost, Route(nameof(Delete))]
        public async Task<HttpMessageModel> Delete(List<Guid> ids)
        {
            return await _" + lowerName + @"Service.DeleteAsync(ids);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name=""model"" ></param>
        /// <returns></returns>
        [HttpPost, Route(nameof(Edit))]
        public async Task<HttpMessageModel> Edit(" + modelName + @"RequestDtoModel model)
        {
            return await _" + lowerName + @"Service.EditAsync(model);
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name=""model"" ></param>
        /// <returns></returns>        
        [HttpPost, Route(nameof(GetListPage))]
        public async Task<HttpMessageModel> GetListPage(" + modelName + @"ListPageRequestModel model)
        {
            return await _" + lowerName + @"Service.ListPageAsync(model);
        }

        /// <summary>
        /// 获得详情
        /// </summary>
        /// <param name=""id"" > 业务编号</param>
        /// <returns></returns>    
        [HttpGet, Route(nameof(GetDetails))]
        public async Task<HttpMessageModel> GetDetails(Guid id)
        {
            return await _" + lowerName + @"Service.GetDetailsAsync(id);
        }


    }
}";
            CreateFile(diskPath + @"\C\" + modelName + "Controller.cs", str);
            //return str;
        }

        public string getIServer(string name, string modelName)
        {
            string str = (@"using JYT.JytDtoModels.WebApiModels;
using JytPlatformServer.DtoModels.BusinessDtoModels;
using JytPlatformServer.DtoModels.BusinessDtoModels.ProjectManagements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JytPlatformServer.IBusiness
{
    /// <summary>
    /// " + name + @"服务接口
    /// </summary>
    public interface I" + modelName + @"Service
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        Task<HttpMessageModel> AddAsync(" + modelName + @"RequestDtoModel model);

        /// <summary>
        /// 删除" + name + @"
        /// </summary>
        /// <param name=""ids""></param>
        /// <returns></returns>
        Task<HttpMessageModel> DeleteAsync(List<Guid> ids);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        Task<HttpMessageModel> EditAsync(" + modelName + @"RequestDtoModel model);

        /// <summary>
        /// 列表查看分页
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        Task<HttpMessageModel> ListPageAsync(" + modelName + @"ListPageRequestModel model);

        /// <summary>
        /// 获得详情
        /// </summary>
        /// <param name=""id""></param>
        /// <returns></returns>
        Task<HttpMessageModel> GetDetailsAsync(Guid id);  
    }
}
");
            CreateFile(diskPath + @"\IS\I" + modelName + "Service.cs", str);
            return str;
        }


        public string getAgainModel(string tableName, string modelName, TypeEnum typeEnum)
        {
            var dbName = new ConfigHelper().Get<string>("mysql:Db");
            var sql = @"select  table_name as TableName,COLUMN_NAME as ColumnName,DATA_Type as ColumnType,column_comment as 'Explain', (Is_Nullable='YES') as IsNullable,character_maximum_length ByteLength
from information_schema.`COLUMNS`
where table_schema = '" + dbName + "' and   table_name='" + tableName + "s'";

            var tcList = db.Ado.SqlQuery<TableColumnsViewModel>(sql).ToList();


            if (typeEnum == TypeEnum.列表)
            {
                var arr = new[] { "LastUpdateEmployeeId", "LastUpdateTime", "EnterpriseId", "IsDelete", "EnterpriseID" };
                tcList = tcList.Where(t => !arr.Contains(t.ColumnName)).ToList();
            }
            else if (typeEnum == TypeEnum.新增)
            {
                var arr = new[] { "Id", "CreateEmployeeId", "CreateTime", "LastUpdateEmployeeId", "LastUpdateTime", "EnterpriseId", "IsDelete", "EnterpriseID" };
                tcList = tcList.Where(t => !arr.Contains(t.ColumnName)).ToList();
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in tcList)
            {
                if (typeEnum == TypeEnum.新增 && item.ColumnType.ToLower() == "tinyint")
                {
                    stringBuilder.AppendLine("           " + item.ColumnName + " = (sbyte)" + modelName + "." + item.ColumnName + ",");
                }
                else
                {
                    stringBuilder.AppendLine("           " + item.ColumnName + " = " + modelName + "." + item.ColumnName + ",");
                }

            }

            return stringBuilder.ToString();
        }

        public string getAgainModel2(string tableName, string modelName, string modelName2)
        {
            var dbName = new ConfigHelper().Get<string>("mysql:Db");
            var sql = @"select  table_name as TableName,COLUMN_NAME as ColumnName,DATA_Type as ColumnType,column_comment as 'Explain', (Is_Nullable='YES') as IsNullable,character_maximum_length ByteLength
from information_schema.`COLUMNS`
where table_schema = '" + dbName + "' and   table_name='" + tableName + "s'";

            var tcList = db.Ado.SqlQuery<TableColumnsViewModel>(sql).ToList();

            var arr = new[] { "Id", "LastUpdateEmployeeId", "LastUpdateTime", "EnterpriseId", "IsDelete", "EnterpriseID" };
            tcList = tcList.Where(t => !arr.Contains(t.ColumnName)).ToList();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in tcList)
            {
                stringBuilder.AppendLine("           " + modelName + "." + item.ColumnName + " = " + modelName2 + "." + item.ColumnName + ";");
            }

            return stringBuilder.ToString();
        }

        public string getModel(string tableName, string modelName)
        {
            //var dbName = new ConfigHelper().Get<string>("sqlserver:Db");


            //            var sql = @"use  " + dbName + @" ;   SELECT d.name TableName,a.colorder 字段序号,a.name  ColumnName,
            //(case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end) IsIdentity,
            //(case when (SELECT count(*) FROM sysobjects  WHERE (name in (SELECT name FROM sysindexes
            //WHERE (id = a.id) AND (indid in  (SELECT indid FROM sysindexkeys  WHERE (id = a.id) AND (colid in  (SELECT colid FROM syscolumns WHERE (id = a.id) AND (name = a.name)))))))  AND (xtype = 'PK'))>0 then '√' else '' end) IsPk,
            //b.name  ColumnType,a.length  ByteLength ,
            //COLUMNPROPERTY(a.id,a.name,'PRECISION') as [Precision],  isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0) as  decimalDigits,
            //(case when a.isnullable=1 then '√'else '' end)  isnullable,isnull(e.text,'')  defaultValue,isnull(g.[value], ' ') AS [explain]
            //FROM  syscolumns a
            //left join systypes b on a.xtype=b.xusertype
            //inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name<>'dtproperties'
            //left join syscomments e on a.cdefault=e.id
            //left join sys.extended_properties g on a.id=g.major_id AND a.colid=g.minor_id
            //left join sys.extended_properties f on d.id=f.class and f.minor_id=0
            //where b.name is not null and d.name='" + tableName + @"'
            //order by a.id,a.colorder";


            var dbName = new ConfigHelper().Get<string>("mysql:Db");
            var sql = @"select  table_name as TableName,COLUMN_NAME as ColumnName,DATA_Type as ColumnType,column_comment as 'Explain', (Is_Nullable='YES') as IsNullable,character_maximum_length ByteLength
from information_schema.`COLUMNS`
where table_schema = '" + dbName + "' and   table_name='" + tableName + "s'";

            var arr = new[] { "EnterpriseId", "IsDelete", "EnterpriseID" };
            var tcList = db.Ado.SqlQuery<TableColumnsViewModel>(sql).Where(t => !arr.Contains(t.ColumnName)).ToList();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in tcList)
            {
                var pType = MySqltoCsharpT(item.ColumnType);
                if (pType.ToLower() == "char" && item.ByteLength.Trim() == "36")
                {
                    pType = "Guid";
                }

                // 生成枚举
                if (pType == "sbyte" && item.ColumnName.ToLower() != "isdelete")
                {
                    if (!string.IsNullOrEmpty(item.Explain))
                    {

                        var splitArr = item.Explain.Split(" ");

                        StringBuilder stringBuilder1 = new StringBuilder();

                        for (int i = 1; i < splitArr.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(splitArr[i]))
                            {
                                var splitValues = splitArr[i].Split(".");

                                stringBuilder1.AppendLine(@"        
        /// <summary>
        /// 
        /// </summary>
        " + splitValues[1] + " = " + splitValues[0] + ", ");
                            }
                        }


                        var enumStr = @"
    /// <summary>
    /// " + splitArr[0] + @"
    /// </summary>
    public enum " + item.ColumnName + @"Enum
    {
" + stringBuilder1.ToString() + @"
    }
";

                        EnumStr = EnumStr + enumStr;

                        if (string.IsNullOrEmpty(EnumName))
                        {
                            EnumName = item.ColumnName + "Enum.cs";
                        }


                        pType = item.ColumnName + "Enum";
                    }
                }

                var dAtt = "";
                if (pType == "string")
                {
                    if (item.IsNullable == "0")
                    {
                        dAtt = "[Required]\n        ";
                    }
                    dAtt += "[MaxLength(" + item.ByteLength + ")]";
                }

                if (item.IsNullable == "1" && (pType != "string"))
                {
                    pType += "?";
                }

                if (dAtt.Length > 0)
                {
                    dAtt += "\n        [Description(\"" + item.Explain + "\")]";
                }
                else
                {
                    dAtt = "[Description(\"" + item.Explain + "\")]";
                }


                stringBuilder.AppendLine(@"
        /// <summary>
        /// " + item.Explain + @"
        /// </summary>      
        " + dAtt + @"
        public " + pType + @" " + item.ColumnName + @" { get; set; }");

            }

            return stringBuilder.ToString();
        }


        /// <summary>
        /// 将Mysql数据类型（如：varchar）转换为.Net类型（如：String）
        /// </summary>
        /// <param name="sqlTypeString">Sql server的数据类型</param>
        /// <returns>相对应的C#数据类型</returns>
        public string MySqltoCsharpT(string sqlType)
        {
            string[] SqlTypeNames = new string[] { "int", "varchar","bit" ,"datetime","decimal","float","image","money",
"ntext","nvarchar","smalldatetime","smallint","text","bigint","binary","char","nchar","numeric",
"real","smallmoney", "sql_variant","timestamp","tinyint","uniqueidentifier","varbinary"};

            string[] CSharpTypes = new string[] {"int", "string","bool" ,"DateTime","Decimal","Double","Byte[]","Single",
"string","string","DateTime","Int16","string","Int64","Byte[]","char","string","Decimal",
"Single","Single", "Object","Byte[]","sbyte","Guid","Byte[]"};

            int i = Array.IndexOf(SqlTypeNames, sqlType.ToLower());

            return CSharpTypes[i];
        }

        /// <summary>
        /// 将SQLServer数据类型（如：varchar）转换为.Net类型（如：String）
        /// </summary>
        /// <param name="sqlTypeString">Sql server的数据类型</param>
        /// <returns>相对应的C#数据类型</returns>
        public string SqltoCsharpT(string sqlType)
        {
            string[] SqlTypeNames = new string[] { "int", "varchar","bit" ,"datetime","decimal","float","image","money",
"ntext","nvarchar","smalldatetime","smallint","text","bigint","binary","char","nchar","numeric",
"real","smallmoney", "sql_variant","timestamp","tinyint","uniqueidentifier","varbinary"};

            string[] CSharpTypes = new string[] {"int", "string","bool" ,"DateTime","Decimal","Double","Byte[]","Single",
"string","string","DateTime","Int16","string","Int64","Byte[]","string","string","Decimal",
"Single","Single", "Object","Byte[]","Byte","Guid","Byte[]"};

            int i = Array.IndexOf(SqlTypeNames, sqlType.ToLower());

            return CSharpTypes[i];
        }

        public void getServer(string name, string modelName)
        {
            var str = @"using JytPlatformServer.Business.Common.Helpers;
using JytPlatformServer.IBusiness;
using JytPlatformServer.Common;
using JytPlatformServer.DbModels.BusinessModels;
using JytPlatformServer.DtoModels.BusinessDtoModels;
using JytPlatformServer.DtoModels.BusinessDtoModels.Chart;
using JytPlatformServer.DtoModels.BusinessDtoModels.Project;
using JytPlatformServer.DtoModels.Common;
using JytPlatformServer.DtoModels.Common.Consts;
using JytPlatformServer.DtoModels.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JYT.JytCommon.CustomAttributes;
using JYT.JytCommon.ExtensionFunctions;
using JYT.JytDtoModels.WebApiModels;
using JytPlatformServer.DtoModels.BusinessModels;
using JytPlatformServer.DtoModels.System;
using JytPlatformServer.DtoModels.UsersDtoModels.Users;
using JytPlatformServer.Business.IServices;
using JytPlatformServer.DbRepositories.BusinessRepositories;

namespace JytPlatformServer.Business.Services
{
    /// <summary>
    /// " + name + @"服务类
    /// </summary>
    [IocRegister(typeof(I" + modelName + @"Service))]
    public class " + modelName + @"Service : BaseService, I" + modelName + @"Service
    {
        #region 属性注入

        /// <summary>
        ///  " + name + @"仓库
        /// </summary>
        public  " + modelName + @"Repository  " + modelName + @"Repository { get; set; }
        
        #endregion        

        #region 增删改查

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> AddAsync(" + modelName + @"RequestDtoModel model)
        {
            var nowTime = DateTime.Now;
            var currentEmployeeId = AuthUserContext.EmployeeId;

             var addEntity = new " + modelName + @"
                {
                    Id = Guid.NewGuid(),
                    CreateEmployeeId = currentEmployeeId,
                    LastUpdateEmployeeId = currentEmployeeId,
                    CreateTime = nowTime,
                    LastUpdateTime = nowTime,
                  " + getAgainModel(modelName, "model", TypeEnum.新增) + @"
                };

            var commandResult = await " + modelName + @"Repository.AddAsync(addEntity);
            return JytHttpMessageModel.SuccessCommand();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> DeleteAsync(List<Guid> ids)
        {
            var list = await " + modelName + @"Repository.GetSelectToListAsync(t => t, t => ids.Contains(t.Id));
             if (list == null || !list.Any())
            {
                 return JytHttpMessageModel.ErrorCommand(""" + name + @"不存在"");
            }

            var currentEmployeeId = AuthUserContext.EmployeeId;
            var nowTime = DateTime.Now;
            list.ForEach(t =>
            {
                t.LastUpdateTime = nowTime;
                t.IsDelete = true;
                t.LastUpdateEmployeeId = currentEmployeeId;
            });

            var commandResult = await " + modelName + @"Repository.EditAsync(list);

            return JytHttpMessageModel.SuccessCommand(commandResult);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> EditAsync(" + modelName + @"RequestDtoModel model)
        {   
            var edit" + modelName + @" = await " + modelName + @"Repository.GetSingleByFilterAsync(t => t.Id == model.Id);
            if (edit" + modelName + @" == null)
            {
                return JytHttpMessageModel.ErrorCommand(""" + name + @"不存在"");
            }
     
            var currentEmployeeId = AuthUserContext.EmployeeId;
            var nowTime = DateTime.Now;
            edit" + modelName + @".LastUpdateTime = nowTime;             
            edit" + modelName + @".LastUpdateEmployeeId = currentEmployeeId;           
             " + getAgainModel2(modelName, " edit" + modelName, "model") + @"
            await " + modelName + @"Repository.EditAsync(edit" + modelName + @");

            return JytHttpMessageModel.SuccessCommand();
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name=""id""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> GetDetailsAsync(Guid id)
        {
            var model = await " + modelName + @"Repository.GetSingleByFilterAsync(t => t.Id == id);
            if ( model == null)
            {
                return JytHttpMessageModel.ErrorCommand(""" + name + @"不存在"");
            }

            var detailResponseModel = new " + modelName + @"DetailsResponseModel()
            {
                " + getAgainModel(modelName, "model", TypeEnum.列表) + @"
            };
          
            return JytHttpMessageModel.SuccessQuery(detailResponseModel);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> ListPageAsync(" + modelName + @"ListPageRequestModel model)
        {
            Expression<Func<" + modelName + @", bool>> predicate = t => t.IsDelete ==false;

            var pageResult = await " + modelName + @"Repository.GetPageAsync(t => new " + modelName + @"ListPageResponseModel
            {
               " + getAgainModel(modelName, "t", TypeEnum.列表) + @"
            }, predicate, t => t.CreateTime, false, model.PageIndex, model.PageSize);

            pageResult.Items.ToList().ForEach(t =>
            {
              
            });
            return JytHttpMessageModel.SuccessQuery(pageResult);
        }   

        #endregion    

        #region 自定义

        #endregion
    }
}";
            //return str;
            CreateFile(diskPath + @"\S\" + modelName + "Service.cs", str);
        }
        public void getServerPlus(string name, string modelName, List<string> sub1List, List<SubItem> sub2List, string other = "")
        {
            var addSubString = ""; var delSubString = ""; var editSubString = "";
            var modelList = new List<string>();

            bool anyfile = false, anymessage = false, anyEmployee = false, anyguidArr = false, anyApproval = false;

            if (!string.IsNullOrEmpty(other))
            {
                var otherList = other.Split(',').ToList();
                anyfile = otherList.Any(t => t == "file");
                anymessage = otherList.Any(t => t == "message");
                anyEmployee = otherList.Any(t => t == "Employee");
                anyguidArr = otherList.Any(t => t == "guid[]");
                anyApproval = otherList.Any(t => t == "Approval");
            }
            if (anyfile)
            {
                addSubString += @"                    
            var fileIdDic = new Dictionary<Guid, List<Guid>>();
            if (model.AttachmentIdList != null && model.AttachmentIdList.Any())
                fileIdDic.Add(editAppraisePlanYear.Id, model.AttachmentIdList);                
";
                editSubString = addSubString;
            }

            var fileStr = "";
            if (anyfile)
            {
                fileStr = @"  
                if (item.AttachmentIdList != null && item.AttachmentIdList.Any())
                        fileIdDic.Add(addEntityAppraisePlan.Id, item.AttachmentIdList);";
            }

            foreach (var sub1 in sub1List)
            {
                modelList.Add(sub1);
                if (!string.IsNullOrEmpty(sub1))
                {
                    delSubString = delSubString + @"      var delete" + sub1 + @"List = await " + sub1 + @"Repository.GetSelectToListAsync(t => t, t => ids.Contains(t." + modelName + @"Id));
            delete" + sub1 + @"List.ForEach(t =>
            {
                t.LastUpdateDateTime = nowTime;
                t.IsDelete = true;
                t.LastUpdateEmployeeId = currentEmployeeId;
            });
            await " + sub1 + @"Repository.EditAsync(delete" + sub1 + @"List, false);
";
                }


                addSubString += @"           
           var add" + sub1 + @"List = new List<" + sub1 + @">();
           foreach (var item in model." + sub1 + @"List)
            {
                var addEntity" + sub1 + @" = new " + sub1 + @"
                {
                    Id = Guid.NewGuid(),
                    CreateEmployeeId = currentEmployeeId,
                    LastUpdateEmployeeId = currentEmployeeId,
                    CreateTime = nowTime,
                    LastUpdateTime = nowTime,                
                    " + modelName + @"Id = addEntity.Id,         
   " + getAgainModel(sub1, "model", TypeEnum.新增) + @"
                };
                add" + sub1 + @"List.Add(addEntity" + sub1 + @");

                " + fileStr + @"
            }

         await " + sub1 + @"Repository.AddRangeAsync(add" + sub1 + @"List, false);

    ";
                editSubString += @" 
            var add" + sub1 + @"List = new List<" + sub1 + @">();
            var edit" + sub1 + @"List = new List<" + sub1 + @">();

            var old" + sub1 + @"List = await " + sub1 + @"Repository.GetSelectToListAsync(t => t, t => t." + modelName + @"Id == model.Id);
            var old" + sub1 + @"IdList = old" + sub1 + @"List.Select(t => t.Id).ToList();

            foreach (var item in model." + sub1 + @"List)
            {
                if (!item.Id.HasValue)
                {
                    var addEntity" + sub1 + @" = new " + sub1 + @"
                    {
                        Id = Guid.NewGuid(),
                        CreateEmployeeId = currentEmployeeId,
                        LastUpdateEmployeeId = currentEmployeeId,
                        CreateTime = nowTime,
                        LastUpdateTime = nowTime,                      
                    };               

                    if (item.AttachmentIdList != null && item.AttachmentIdList.Any())
                        fileIdDic.Add(addEntity" + sub1 + @".Id, item.AttachmentIdList);
                }
                else
                {
                    // 编辑
                    var edit" + sub1 + @" = old" + sub1 + @"List.FirstOrDefault(st => st.Id == item.Id);
                    edit" + sub1 + @".LastUpdateEmployeeId = currentEmployeeId;
                    edit" + sub1 + @".LastUpdateTime = nowTime;                
      
                    edit" + sub1 + @"List.Add(edit" + sub1 + @");

                    if (item.AttachmentIdList != null && item.AttachmentIdList.Any())
                        fileIdDic.Add(edit" + sub1 + @".Id, item.AttachmentIdList);
                }
            }

           // 删除
            var delete" + sub1 + @"IdList = old" + sub1 + @"IdList.Except(model." + sub1 + @"List.Where(t => t.Id.HasValue).Select(t => t.Id.Value)).ToList();
            var deleteOld" + sub1 + @"List = old" + sub1 + @"List.Where(t => delete" + sub1 + @"IdList.Contains(t.Id)).ToList();
            deleteOld" + sub1 + @"List.ForEach(t =>
            {
                t.LastUpdateDateTime = nowTime;
                t.IsDelete = true;
                t.LastUpdateEmployeeId = currentEmployeeId;
            });  
";


            }

            foreach (var sub2 in sub2List)
            {
                var sub1 = sub2.Item;
                modelList.Add(sub1);

                delSubString = delSubString + @"
            var " + sub2.Parent.FirstCharToLower() + @"IdList = delete" + sub2.Parent + @"List.Select(t => t.Id).ToList();

            var delete" + sub1 + @"List = await " + sub1 + @"Repository.GetSelectToListAsync(t => t, t => " + sub2.Parent.FirstCharToLower() + @"IdList.Contains(t." + sub2.Parent + @"Id));
            delete" + sub1 + @"List.ForEach(t =>
            {
                t.LastUpdateDateTime = nowTime;
                t.IsDelete = true;
                t.LastUpdateEmployeeId = currentEmployeeId;
            });
            await " + sub1 + @"Repository.EditAsync(delete" + sub1 + @"List, false);";


                addSubString += @"           
           var add" + sub1 + @"List = new List<" + sub1 + @">();
           foreach (var item in model." + sub1 + @"List)
            {
                var addEntity" + sub1 + @" = new " + sub1 + @"
                {
                    Id = Guid.NewGuid(),
                    CreateEmployeeId = currentEmployeeId,
                    LastUpdateEmployeeId = currentEmployeeId,
                    CreateTime = nowTime,
                    LastUpdateTime = nowTime,                
                    " + modelName + @"Id = addEntity.Id,       
" + getAgainModel(sub1, "model", TypeEnum.新增) + @"
                };
                add" + sub1 + @"List.Add(addEntity" + sub1 + @");
     " + fileStr + @"
            }

         await " + sub1 + @"Repository.AddRangeAsync(add" + sub1 + @"List, false);

    ";

                editSubString += @"
                     var " + sub1.FirstCharToLower() + @"List = await " + sub1 + @"Repository.GetSelectToListAsync(t => t, t => old" + sub2.Parent + @"IdList.Contains(t." + sub2.Parent + @"Id));
                     var add" + sub1 + @"List = new List<" + sub1 + @">();
                     var del" + sub1 + @"List = new List<" + sub1 + @">();

                    var requestIdList=new List<Guid>();
                    
                      var currentOldObjectEmployeeId = " + sub1.FirstCharToLower() + @"List
                   .Where(st => st." + sub2.Parent + @"Id == edit" + sub2.Parent + @".Id).Select(st => st.SelfRatingEmployeeId).ToList();

                    // 删除
                    var deleteObjectIdList = currentOldObjectEmployeeId.Except(requestIdList).ToList();
                    if (deleteObjectIdList.Any())
                    {
                        var deleteObj = " + sub1.FirstCharToLower() + @"List.Where(st => st." + sub2.Parent + @"Id == item.Id && deleteObjectIdList.Contains(st.Id));
                        del" + sub1 + @"List.AddRange(deleteObj);
                    }

                    // 新增
                    var addObjectList = requestIdList.Except(currentOldObjectEmployeeId);
                    if (addObjectList.Any())
                    {
                        foreach (var modelEmployeeId in addObjectList)
                        {
                            var add" + sub1 + @"Entity = new " + sub1 + @"
                            {
                                Id = Guid.NewGuid(),
                                CreateEmployeeId = currentEmployeeId,
                                LastUpdateEmployeeId = currentEmployeeId,
                                CreateTime = nowTime,
                                LastUpdateTime = nowTime,
                             
                            };
                            add" + sub1 + @"List.Add(add" + sub1 + @"Entity);

                            nowTime = nowTime.AddSeconds(10);
                        }
                    } 

                    del" + sub1 + @"List.AddRange(" + sub1 + @"List.Where(st => " + sub2.Parent + @"IdList.Contains(st." + sub2.Parent + @"Id)).ToList());
                    del" + sub1 + @"List.ForEach(t =>
                    {
                        t.LastUpdateDateTime = nowTime;
                        t.IsDelete = true;
                        t.LastUpdateEmployeeId = currentEmployeeId;
                    });

            await " + sub1 + @"Repository.EditAsync(del" + sub1 + @"List, false);       
            await " + sub1 + @"Repository.AddRangeAsync(add" + sub1 + @"List, false);";

            }

            var sss = "";
            foreach (var modelName1 in modelList)
            {
                sss += @"       

        /// <summary>
        /// 
        /// </summary>
        public  " + modelName1 + @"Repository  " + modelName1 + @"Repository { get; set; }";
            }


            if (anyApproval)
            {
                sss += (@"    
        /// <summary>
        /// 审批服务
        /// </summary>
        public ApprovalService ApprovalService { get; set; }
");

                addSubString += @"
            var approval = await ApprovalService.AddAsync(addEntity.Id, model.Approval, ApprovalModuleTypeEnum.项目研发, ApprovalModuleSubtypeTypeEnum.变更项目, addEntity.ChangeNumber);"
;
            }

            if (anyEmployee)
            {
                ;
            }

            if (anyfile)
            {
                sss += (@"    
        /// <summary>
        /// 文件服务类
        /// </summary>
        public FileService FileService { get; set; }");

                addSubString += @" 
              await FileService.AddBusinesFileAsync(fileIdDic);
";

                editSubString += @" 
                 await FileService.AddBusinesFileAsync(fileIdDic);
  "
                    ;
            }

            if (anyguidArr)
            {

            }

            if (anymessage)
            {
                sss += (@"
         /// <summary>
         /// 
         /// </summary>
        public SystemMessageService SystemMessageService { get; set; }
");

                addSubString += @" 
                       
            #region 生成消息

            var messageContent = ""消息内容"";

            var messages = new List<SystemMessageAddRequestModel>();
                foreach (var item in appraisePersonalList)
                {
                    messages.Add(new SystemMessageAddRequestModel
                    {
                        MessageContent = messageContent,
                        SubordinateModuleBusinessId = item.Id,
                        MessageType = SystemMessageTypeEnum.提交个人承诺,
                        SubordinateModule = SubordinateModuleEnum.个人工作台,
                        ReceptionEmployeeIdList = new List<Guid>() { }
                    });
                }

                await SystemMessageService.AddAsync(messages);
            #endregion
                ";
            }


            var str = @"using JytPlatformServer.Business.Common.Helpers;
using JytPlatformServer.IBusiness;
using JytPlatformServer.Common;
using JytPlatformServer.DbModels.BusinessModels;
using JytPlatformServer.DtoModels.BusinessDtoModels;
using JytPlatformServer.DtoModels.BusinessDtoModels.Chart;
using JytPlatformServer.DtoModels.BusinessDtoModels.Project;
using JytPlatformServer.DtoModels.Common;
using JytPlatformServer.DtoModels.Common.Consts;
using JytPlatformServer.DtoModels.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JYT.JytCommon.CustomAttributes;
using JYT.JytCommon.ExtensionFunctions;
using JYT.JytDtoModels.WebApiModels;
using JytPlatformServer.DtoModels.BusinessModels;
using JytPlatformServer.DtoModels.System;
using JytPlatformServer.DtoModels.UsersDtoModels.Users;
using JytPlatformServer.Business.IServices;
using JytPlatformServer.DbRepositories.BusinessRepositories;

namespace JytPlatformServer.Business.Services
{
    /// <summary>
    /// " + name + @"服务类
    /// </summary>
    [IocRegister(typeof(I" + modelName + @"Service))]
    public class " + modelName + @"Service : BaseService, I" + modelName + @"Service
    {
        #region 属性注入

        /// <summary>
        ///  " + name + @"仓库
        /// </summary>
        public  " + modelName + @"Repository  " + modelName + @"Repository { get; set; }        
" + sss + @"
        #endregion        

        #region 增删改查

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> AddAsync(" + modelName + @"RequestDtoModel model)
        {
            var nowTime = DateTime.Now;
            var currentEmployeeId = AuthUserContext.EmployeeId;

             var addEntity = new " + modelName + @"
                {
                    Id = Guid.NewGuid(),
                    CreateEmployeeId = currentEmployeeId,
                    LastUpdateEmployeeId = currentEmployeeId,
                    CreateTime = nowTime,
                    LastUpdateTime = nowTime,
                  " + getAgainModel(modelName, "model", TypeEnum.新增) + @"
                };

              " + addSubString + @"
            var commandResult = await " + modelName + @"Repository.AddAsync(addEntity);
            return JytHttpMessageModel.SuccessCommand();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> DeleteAsync(List<Guid> ids)
        {
            var list = await " + modelName + @"Repository.GetSelectToListAsync(t => t, t => ids.Contains(t.Id));
             if (list == null || !list.Any())
            {
                 return JytHttpMessageModel.ErrorCommand(""" + name + @"不存在"");
            }

            var currentEmployeeId = AuthUserContext.EmployeeId;
            var nowTime = DateTime.Now;
            list.ForEach(t =>
            {
                t.LastUpdateTime = nowTime;
                t.IsDelete = true;
                t.LastUpdateEmployeeId = currentEmployeeId;
            });

 " + delSubString + @"
            var commandResult = await " + modelName + @"Repository.EditAsync(list);

            return JytHttpMessageModel.SuccessCommand(commandResult);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> EditAsync(" + modelName + @"RequestDtoModel model)
        {   
            var edit" + modelName + @" = await " + modelName + @"Repository.GetSingleByFilterAsync(t => t.Id == model.Id);
            if (edit" + modelName + @" == null)
            {
                return JytHttpMessageModel.ErrorCommand(""" + name + @"不存在"");
            }
     
            var currentEmployeeId = AuthUserContext.EmployeeId;
            var nowTime = DateTime.Now;
            edit" + modelName + @".LastUpdateTime = nowTime;             
            edit" + modelName + @".LastUpdateEmployeeId = currentEmployeeId;           
             " + getAgainModel2(modelName, " edit" + modelName, "model") + @"

   " + editSubString + @"
            await " + modelName + @"Repository.EditAsync(edit" + modelName + @");

            return JytHttpMessageModel.SuccessCommand();
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name=""id""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> GetDetailsAsync(Guid id)
        {
            var model = await " + modelName + @"Repository.GetSingleByFilterAsync(t => t.Id == id);
            if ( model == null)
            {
                return JytHttpMessageModel.ErrorCommand(""" + name + @"不存在"");
            }

            var detailResponseModel = new " + modelName + @"DetailsResponseModel()
            {
                " + getAgainModel(modelName, "model", TypeEnum.列表) + @"
            };
          
            return JytHttpMessageModel.SuccessQuery(detailResponseModel);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> ListPageAsync(" + modelName + @"ListPageRequestModel model)
        {
            Expression<Func<" + modelName + @", bool>> predicate = t => t.IsDelete ==false;

            var pageResult = await " + modelName + @"Repository.GetPageAsync(t => new " + modelName + @"ListPageResponseModel
            {
               " + getAgainModel(modelName, "t", TypeEnum.列表) + @"
            }, predicate, t => t.CreateTime, false, model.PageIndex, model.PageSize);

            pageResult.Items.ToList().ForEach(t =>
            {
              
            });
            return JytHttpMessageModel.SuccessQuery(pageResult);
        }   

        #endregion    

        #region 自定义

        #endregion
    }
}";
            //return str;
            CreateFile(diskPath + @"\Plus\" + modelName + "Service.cs", str);
        }
        /// <summary>
        /// 获得视图模型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="modelName"></param>
        public void getViewModel(string name, string modelName)
        {
            var propStr = getModel(modelName, "");

            string entityStr = @"using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JytPlatformServer.DbModels;

namespace JytPlatformServer.DbModels.BusinessModels
{
    /// <summary>
    /// " + name + @"
    /// </summary>
    public class " + modelName + @" : BaseJytPlatformServerDbModel
    {
" + propStr + @"
          /// <summary>
          ///  
          /// </summary>
          public override bool IsDelete { get; set; }

          /// <summary>
          /// 
          /// </summary>
          public override Guid EnterpriseId { get; set; }
    }
}
";
            CreateFile(diskPath + @"\M\" + modelName + ".cs", entityStr);




            var str = @"using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JytPlatformServer.DtoModels.Common.Enums;

namespace JytPlatformServer.DtoModels.BusinessDtoModels
{
    /// <summary>
    /// " + name + @"编辑模型
    /// </summary>
    public class " + modelName + @"RequestDtoModel
    {
" + propStr + @"
    }

";

            str += @"   
    /// <summary>
    /// " + name + @"详情响应模型
    /// </summary>
    public class " + modelName + @"DetailsResponseModel : " + modelName + @"RequestDtoModel
    {
    }     
    
    /// <summary>
    /// " + name + @"分页列表请求模型
    /// </summary>
    public class " + modelName + @"ListPageRequestModel
    {
        /// <summary>
        /// 自然数页码 索引值 从1 开始 
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

     /// <summary>
    /// " + name + @"列表响应模型
    /// </summary>
    public class " + modelName + @"ListPageResponseModel  : " + modelName + @"DetailsResponseModel
    {
    }     
}
";

            CreateFile(diskPath + @"\V\" + modelName + "RequestDtoModel.cs", str);

        }



        public enum TypeEnum
        {
            新增 = 1,
            列表
        }

    }

    public class SubItem
    {
        public string Parent { get; set; }

        public string Item { get; set; }

    }

    public static class AAA
    {

        /// <summary>
        /// 首字母小写写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToLower() + input.Substring(1);
            return str;

        }

    }
}
