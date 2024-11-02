using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class ArtistController : Controller
    {
        private readonly NhaccuatuiModel db = new NhaccuatuiModel();
        // GET: Artist
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddArtist(string name, string country, string bio)
        {
            db.get($"INSERT INTO Artists (Name, Bio, Country) VALUES (N'{name}', N'{bio}', N'{country}')");
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeleteArtist(int id)
        {
            db.get($"DELETE FROM Artists WHERE ArtistID = {id}");
            return RedirectToAction("Index", "Admin");
        }
    }
}