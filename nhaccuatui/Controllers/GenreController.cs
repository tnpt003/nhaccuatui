using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class GenreController : Controller
    {
        // GET: Genre 

        public ActionResult AddGenre(string name)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Add new genre
            db.get($"EXEC AddGenre @Name = '{name}'");

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeleteGenre(int id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            // Execute delete stored procedure
            ViewBag.list = db.get("EXEC DeleteGenre " + id);
            return RedirectToAction("Index", "Admin");
        }
    }

}