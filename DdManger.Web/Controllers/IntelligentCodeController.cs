using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdManger.Web.Models;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.IO;

namespace DdManger.Web.Controllers
{
    /// <summary>
    /// 智能代码
    /// </summary>
    public class IntelligentCodeController : Controller
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
        public IntelligentCodeController()
        {
            diskPath = @"D:\IntelligentCode\" + DateTime.Now.ToString("yyyyMMdd");

            if (!Directory.Exists(diskPath))
            {
                Directory.CreateDirectory(diskPath + @"\R");
                Directory.CreateDirectory(diskPath + @"\V");
            }
        }

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


                //var ddd = getModel("t_accounts", "");
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

namespaceJytPlatformServer.DbRepositories.BusinessRepositories
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
using JytPlatformServer.DtoModels.BusinessDtoModels.Milestone;
using JytPlatformServer.DtoModels.BusinessDtoModels.ProjectManagements;
using JytPlatformServer.WebAPI.Business.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JytPlatformServer.WebAPI.Controllers.BusinessControllers
{
    /// <summary>
    /// " + name + @"控制器
    /// </summary>
    public class " + modelName + @"Controller : BaseBusinessJytPlatformServerCommonController
    {
        readonly I" + modelName + @"Service _milestoneService;

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
        public async Task<HttpMessageModel> Add(" + modelName + @"EditRequestModel model)
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
        public async Task<HttpMessageModel> Edit(" + modelName + @"EditRequestModel model)
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

        /// <summary>
        ///  转审（指派）
        /// </summary>
        /// <param name=""request"" ></param>
        /// <returns></returns>
        [HttpPost, Route(nameof(Assign))]
        public async Task<HttpMessageModel> Assign(" + modelName + @"AssignRequestDto request)
        {
            return await _" + lowerName + @"Service.AssignAsync(request);
        }

        /// <summary>
        /// 审批
        /// <para>带有发送消息</para>
        /// </summary>
        /// <param name=""approvalTaskRequestModel"" ></param>
        /// <returns></returns>
        [HttpPost, Route(nameof(NodeAudit))]
        public async Task<HttpMessageModel> NodeAudit(" + modelName + @"NodeAuditDtoModelRequestModel approvalTaskRequestModel)
        {
            return await_" + lowerName + @"Service.NodeAuditAsync(approvalTaskRequestModel);
        }
    }
}";
            CreateFile(diskPath + @"\" + modelName + "Controller.cs", str);
            //return str;
        }

        public string getIServer(string name, string modelName)
        {
            string str = (@"using JYT.JytDtoModels.WebApiModels;
using JytPlatformServer.DtoModels.BusinessDtoModels.Milestone;
using JytPlatformServer.DtoModels.BusinessDtoModels.ProjectManagements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JytPlatformServer.Business.IServices
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
        Task<HttpMessageModel> AddAsync(" + modelName + @"EditRequestModel model);

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
        Task<HttpMessageModel> EditAsync(" + modelName + @"EditRequestModel model);

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

        /// <summary>
        /// 转审（指派）
        /// </summary>
        /// <param name=""request""></param>
        /// <returns></returns>
        Task<HttpMessageModel> AssignAsync(" + modelName + @"AssignRequestDto request);

        /// <summary>
        /// 审批
        /// </summary>
        /// <param name=""approvalTaskRequestModel""></param>
        /// <returns></returns>
        Task<HttpMessageModel> NodeAuditAsync(" + modelName + @"NodeAuditDtoModelRequestModel approvalRequestModel);
    }
}
");
            CreateFile(diskPath + @"\I" + modelName + "Service.cs", str);
            return str;
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
            var sql = @"select  table_name as TableName,COLUMN_NAME as ColumnName,Column_Type as ColumnType,column_comment as 'Explain'
from information_schema.`COLUMNS`
where table_schema = '" + dbName + "' and   table_name='" + tableName + "s'";

            var tcList = db.Ado.SqlQuery<TableColumnsViewModel>(sql).ToList();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in tcList)
            {
                stringBuilder.AppendLine(@"
        /// <summary>
        /// " + item.Explain + @"
        /// </summary>
        [Description(""" + item.Explain + @""")]
        public string " + item.ColumnName + @" { get; set; }");

            }

            return stringBuilder.ToString();
        }

        public void getServer(string name, string modelName)
        {
            var str = @"using JYT.JytCommon;
using JYT.JytCommon.CustomAttributes;
using JYT.JytCommon.ExtensionFunctions;
using JYT.JytDtoModels.WebApiModels;
using JytPlatformServer.Business.Common.Helpers;
using JytPlatformServer.Business.Common.JPush;
using JytPlatformServer.Business.Common.SignalR;
using JytPlatformServer.Business.IServices;
using JytPlatformServer.Common;
using JytPlatformServer.DbModels.BusinessModels;
using JytPlatformServer.DbRepositories.BusinessRepositories;
using JytPlatformServer.DbRepositories.BusinessRepositories.OrganizationsRepositories;
using JytPlatformServer.DbRepositories.BusinessRepositories.Other;
using JytPlatformServer.DbRepositories.BusinessRepositories.ProjectManagementRepositories;
using JytPlatformServer.DtoModels.BusinessDtoModels;
using JytPlatformServer.DtoModels.BusinessDtoModels.Flows;
using JytPlatformServer.DtoModels.BusinessDtoModels.Messages;
using JytPlatformServer.DtoModels.BusinessDtoModels.Milestone;
using JytPlatformServer.DtoModels.BusinessDtoModels.ProjectManagements;
using JytPlatformServer.DtoModels.BusinessDtoModels.TaskManagements;
using JytPlatformServer.DtoModels.Common;
using JytPlatformServer.DtoModels.Common.Consts;
using JytPlatformServer.DtoModels.Common.Enums;
using JytPlatformServer.DtoModels.UsersDtoModels.Users;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace JytPlatformServer.Business.Services
{
    /// <summary>
    /// " + name + @"服务类
    /// </summary>
    [IocRegister(typeof(I" + modelName + @"Service))]
    public class " + modelName + @"Service : BaseService, I" + modelName + @"Service
    {
        #region 属性注入

        
        #endregion        

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> AddAsync(" + modelName + @"EditRequestModel model)
        {
            return JytHttpMessageModel.SuccessCommand();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name=""ids""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> DeleteAsync(List<Guid> ids)
        {
            var list = await Current" + modelName + @"Repository.GetSelectToListAsync(t => t, t => ids.Contains(t." + modelName + @"Id));

            var nowTime = DateTime.Now;
            list.ForEach(t =>
            {
                t.LastUpdateDateTime = nowTime;
                t.IsDelete = true;
            });

            var commandResult = await Current" + modelName + @"Repository.EditAsync(list);

            return JytHttpMessageModel.SuccessCommand(commandResult);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> EditAsync(" + modelName + @"EditRequestModel model)
        {     

            var edit" + modelName + @" = await Current" + modelName + @"Repository.GetSingleByFilterAsync(t => t." + modelName + @"Id == model." + modelName + @"Id);
            if (editMilestone == null)
            {
                return JytHttpMessageModel.ErrorCommand(MessageCodeTypeEnum." + modelName + @"NotFound);
            }
     
            var nowTime = DateTime.Now;
            edit" + modelName + @".LastUpdateDateTime = nowTime;
            edit" + modelName + @".Name = model.Name;       
 
            await Current" + modelName + @"Repository.EditAsync(edit" + modelName + @");

            return JytHttpMessageModel.SuccessCommand();
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name=""id""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> GetDetailsAsync(Guid id)
        {
            var milestone = await Current" + modelName + @"Repository.GetSingleByFilterAsync(t => t." + modelName + @"Id == id);
            if (milestone == null)
            {
                return JytHttpMessageModel.ErrorCommand(MessageCodeTypeEnum." + modelName + @"NotFound);
            }

            var " + modelName + @"ResponseModel = new " + modelName + @"DetailsResponseModel()
            {
              
            };
          
            return JytHttpMessageModel.SuccessQuery(milestoneResponseModel);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name=""model""></param>
        /// <returns></returns>
        public async Task<HttpMessageModel> ListPageAsync(" + modelName + @"ListPageRequestModel model)
        {
            Expression<Func<T" + modelName + @", bool>> predicate = t => milestoneIds.Contains(t.ProjectManagementMilestoneId);

            var pageResult = await Current" + modelName + @"Repository.GetPageAsync(t => new " + modelName + @"ListPageResponseModel
            {
                ProjectManagementMilestoneId = t.ProjectManagementMilestoneId,
                Deliverable = t.Deliverable,
                ExpirationReminderThreshold = t.ExpirationReminderThreshold,
                PlannedStartTime = t.PlannedStartTime,
                PlannedEndTime = t.PlannedEndTime,
                ActualStartTime = t.ActualStartTime,
                ActualEndTime = t.ActualEndTime,
                Name = t.Name,
                MilestoneStatus = t.MilestoneStatus,
                Explain = t.Explain,
                MakerEmployeeIdlistStr = t.MakerEmployeeIdList,
                ProgressValue = t.ProgressValue
            }, predicate, t => t.CreateDateTime, false, model.PageIndex, model.PageSize);

            pageResult.Items.ToList().ForEach(t =>
            {
              
            });
            return JytHttpMessageModel.SuccessQuery(pageResult);
        }     
    }
}";
            //return str;
            CreateFile(diskPath + @"\" + modelName + "Service.cs", str);
        }

        /// <summary>
        /// 获得视图模型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="modelName"></param>
        public void getViewModel(string name, string modelName)
        {

            var propStr = getModel(modelName, "");

            string str = @"using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JytPlatformServer.DtoModels.BusinessModels
{
    /// <summary>
    /// " + name + @"
    /// </summary>
    public class " + modelName + @" : BaseJytPlatformServerDbModel
    {
" + propStr + @"
    }
}
";
            CreateFile(diskPath + @"\" + modelName + ".cs", str);


            str = @"using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JytPlatformServer.DtoModels.Common.Enums;

namespace JytPlatformServer.DtoModels.BusinessDtoModels
{
    /// <summary>
    /// " + name + @"编辑模型
    /// </summary>
    public class " + modelName + @"EditRequestModel
    {
    }
}
";
            CreateFile(diskPath + @"\V\" + modelName + "RequestDtoModel.cs", str);

            str = @"using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JytPlatformServer.DtoModels.Common.Enums;

namespace JytPlatformServer.DtoModels.BusinessDtoModels
{
    /// <summary>
    /// " + name + @"列表响应模型
    /// </summary>
    public class " + modelName + @"ListResponseModel
    {
    }
}
";
            CreateFile(diskPath + @"\V\" + modelName + "ListResponseModel.cs", str);


            str = @"using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JytPlatformServer.DtoModels.Common.Enums;

namespace JytPlatformServer.DtoModels.BusinessDtoModels
{
    /// <summary>
    /// " + name + @"详情响应模型
    /// </summary>
    public class " + modelName + @"DetailsResponseModel
    {
    }
}
";
            CreateFile(diskPath + @"\V\" + modelName + "DetailsResponseModel.cs", str);

        }
    }
}