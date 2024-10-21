using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using nhaccuatui.Models;

namespace nhaccuatui.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            ViewBag.list = db.get("SELECT * FROM Songs");
            return View();
        }
    }
}