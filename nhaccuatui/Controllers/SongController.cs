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
    HttpPostedFileBase fileUrl,
    HttpPostedFileBase imageUrl,
    int albumId,
    int genreId)
        {
            try
            {
                string audioFilename = null;
                string imageFilename = null;

                // Process image file upload
                if (imageUrl != null && imageUrl.ContentLength > 0)
                {
                    imageFilename = Path.GetFileName(imageUrl.FileName);
                    string imagePath = Path.Combine(Server.MapPath("~/Image/Data"), imageFilename);
                    imageUrl.SaveAs(imagePath);
                }

                // Process audio file upload
                if (fileUrl != null && fileUrl.ContentLength > 0)
                {
                    audioFilename = Path.GetFileName(fileUrl.FileName);
                    string audioPath = Path.Combine(Server.MapPath("~/Audio"), audioFilename);
                    fileUrl.SaveAs(audioPath);
                }

                // Instantiate the database model
                NhaccuatuiModel db = new NhaccuatuiModel();

                // Execute stored procedure to add a new song and retrieve the updated list
                ViewBag.list = db.get("EXEC AddSong N'" + title + "', '" + duration + "', '" + releaseDate + "', '" + audioFilename + "', '" + imageFilename + "', " + albumId + ", " + genreId + ";");
            }
            catch (Exception ex)
            {
                // Handle the exception (optional: log or display the error message)
            }

            // Redirect to the Index action in the Admin controller after adding the song
            return RedirectToAction("Index", "Admin");
        }


    }
}