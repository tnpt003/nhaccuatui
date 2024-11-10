using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nhaccuatui.Models;

namespace nhaccuatui.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TrangChu()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Core()
        {
            ViewBag.Message = "Your core page.";

            return View();
        }
        // Search Action
        public ActionResult SearchSongsByTitle(string title)
        {
            // Initialize the database model
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Use the EXEC command with the provided title parameter
            ViewBag.list = db.get($"EXEC SearchSongsByTitle '{title}'");

            // Return the SearchResults view
            return View("SearchResults");
        }




    }
}