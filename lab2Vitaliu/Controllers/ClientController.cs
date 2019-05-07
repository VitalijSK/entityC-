using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lab2Vitaliu.Filters;

namespace lab2Vitaliu.Controllers
{
    [Route("client")]
    [ServiceFilter(typeof(LoggerFilter))]
    [ServiceFilter(typeof(CustomExceptionFilterAttribute))]
    public class ClientController : Controller
    {  
        public IActionResult Index()
        {
            return View();
        }
    }
}