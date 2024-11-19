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
    }
}