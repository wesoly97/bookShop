using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bookShopProject.Models;
namespace bookShopProject.Controllers
{
    public class homeController : Controller
    {
        
        // GET: home
        public ActionResult Index()
        {
            return View();
        }
    }

       
}
