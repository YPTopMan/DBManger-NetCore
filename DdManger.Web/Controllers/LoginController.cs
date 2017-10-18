using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DdManger.Web.Models;

namespace DdManger.Web.Controllers
{
    /// <summary>
    /// 登陆控制器
    /// <para>信息都存入cookie 中</para>
    /// </summary>
    public class LoginController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 保存登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(DBLoginViewModel viewModel)
        {
            var cookieValue = Request.Cookies["dbAddress"];

            // 写入到cookie
            var cookieKey = "dbAddress";

            if (!string.IsNullOrEmpty(cookieValue))
            {
                cookieValue = cookieValue + "," + viewModel.Address + "|" + viewModel.Name + "|" + viewModel.Password;
            }
            else
            {
                cookieValue = viewModel.Address + "|" + viewModel.Name + "|" + viewModel.Password;
            }

            HttpContext.Response.Cookies.Append(cookieKey, cookieValue);

            return View();
        }
    }
}