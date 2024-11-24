using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using nhaccuatui.Models;

namespace nhaccuatui.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        private readonly NhaccuatuiModel db = new NhaccuatuiModel();
        public ActionResult Index()
        {
            // Initialize the database model
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Get data from the Songs table (example)
            var songs = db.get(@"
        SELECT 
            s.SongID, 
            s.Title, 
            s.ReleaseDate, 
            s.Duration, 
            s.ImageUrl, 
            s.FileUrl, 
            g.Name AS GenreName, 
            al.Title AS AlbumTitle, 
            ar.Name AS ArtistName 
        FROM Songs s
        LEFT JOIN Genres g ON s.GenreID = g.GenreID
        LEFT JOIN Albums al ON s.AlbumID = al.AlbumID
        LEFT JOIN Artists ar ON al.ArtistID = ar.ArtistID
    ");

            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Songs()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Songs");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Albums()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Albums");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Artists()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Artists");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Users()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Users");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Genres()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Genres");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Playlists()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Playlists");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PlaylistSongs()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM PlaylistSongs");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Likes()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Likes");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Comments()
        {
            // Get data from the Songs table (example)
            var songs = db.get("SELECT * FROM Comments");
            // Convert the list into a JSON array (just the Songs data)
            return Json(songs, JsonRequestBehavior.AllowGet);
        }
    }
}