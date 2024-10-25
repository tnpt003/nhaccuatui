using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class SongController : Controller
    {
        // GET: Song
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeleteSong(string id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            ViewBag.list = db.get("EXEC DeleteSong " + id);
            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public ActionResult AddSong(
    string title,
    string duration,
    string releaseDate,
    string fileUrl,
    string imageUrl,
    int albumId,
    int genreId)
        {
            // Instantiate the database model
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Execute stored procedure to add a new song
            db.get($"EXEC AddSong @Title = '{title}', @Duration = '{duration}', @ReleaseDate = '{releaseDate}', @FileUrl = '{fileUrl}', @ImageUrl = '{imageUrl}', @AlbumID = {albumId}, @GenreID = {genreId}");

            // Redirect to the Index action in the Admin controller after adding the song
            return RedirectToAction("Index", "Admin");
        }



    }
}