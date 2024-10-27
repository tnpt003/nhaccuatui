using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class SongController : Controller
    {
        // GET: Song
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
    HttpPostedFileBase imageUrl,
    int albumId,
    int genreId)
        {
            try
            {
                if (imageUrl != null && imageUrl.ContentLength > 0)
                {
                    string filename = Path.GetFileName(imageUrl.FileName);
                    string path = Path.Combine(Server.MapPath("~/Image/Data"), filename);
                    imageUrl.SaveAs(path);
                    // Instantiate the database model
                    NhaccuatuiModel db = new NhaccuatuiModel();

                    // Execute stored procedure to add a new song
                    db.get($"EXEC AddSong N'{title}', '{duration}', '{releaseDate}', '{fileUrl}', '{filename}', {albumId}, {genreId};");

                }
            }
            catch (Exception) { }
            // Redirect to the Index action in the Admin controller after adding the song
            return RedirectToAction("Index", "Admin");
        }

    }
}