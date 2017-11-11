using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DdManger.Web.Controllers
{
    public class OracleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}