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
            ViewBag.list = db.get("SELECT * FROM Songs ORDER BY SONGID DESC");
            ViewBag.listUs = db.get("SELECT * FROM Users ORDER BY USERID DESC");
            ViewBag.listAr = db.get("SELECT * FROM Artists ORDER BY ARTISTID DESC");
            ViewBag.listAl = db.get("SELECT * FROM Albums ORDER BY ALBUMID DESC");
            ViewBag.listGe = db.get("SELECT * FROM Genres ORDER BY GENREID DESC");
            ViewBag.listPl = db.get("SELECT * FROM Playlists ORDER BY PLAYLISTID DESC");
            ViewBag.listPlS = db.get("SELECT * FROM PlaylistSongs ORDER BY PLAYLISTID DESC");
            ViewBag.listLi = db.get("SELECT * FROM Likes ORDER BY LIKEID DESC");
            ViewBag.listCo = db.get("SELECT * FROM Comments ORDER BY COMMENTID DESC");
            return View();
        }
    }
}