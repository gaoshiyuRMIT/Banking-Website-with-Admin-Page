using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace Banking.Controllers
{
    public class ErrorController : Controller
    {
        //GET: /<controller>/
        [HttpGet("/Error/{error}")]
        public IActionResult Index(int error)
        {
            return View(error);
        }
    }
}




