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

        [HttpPost]
        public ActionResult UpdateSong(
    int songId,  // Song ID to update
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
                else
                {
                    // Handle the case where the image is not uploaded
                    // You might want to retrieve the existing image URL from the database if the image is not updated
                    // For this, you can add logic to fetch the current image URL based on songId
                }

                // Process audio file upload
                if (fileUrl != null && fileUrl.ContentLength > 0)
                {
                    audioFilename = Path.GetFileName(fileUrl.FileName);
                    string audioPath = Path.Combine(Server.MapPath("~/Audio"), audioFilename);
                    fileUrl.SaveAs(audioPath);
                }
                else
                {
                    // Handle the case where the audio file is not uploaded
                    // Similar logic can be applied to fetch the existing audio URL if needed
                }

                // Instantiate the database model
                NhaccuatuiModel db = new NhaccuatuiModel();

                // Execute stored procedure to update the song
                db.get($"EXEC UpdateSong {songId}, N'{title}', '{duration}', '{releaseDate}', '{audioFilename}', '{imageFilename}', {albumId}, {genreId};");
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
            }

            // Redirect to the Index action in the Admin controller after updating the song
            return RedirectToAction("Index", "Admin");
        }



        public ActionResult GetSongUpdate(int id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Ensure that the stored procedure returns the song data
            var songData = db.get($"EXEC GetSongById {id}");

            if (songData != null && songData.Count > 0)
            {
                ViewBag.SongData = songData; // Store song data in ViewBag
                ViewBag.listAl = db.get("SELECT * FROM Albums ORDER BY ALBUMID DESC");
                ViewBag.listGe = db.get("SELECT * FROM Genres ORDER BY GENREID DESC");

                // Make sure to return the correct path to the UpdateSong view in the Admin folder
                return View("~/Views/Admin/UpdateSong.cshtml");
            }

            // If no song was found, redirect back to the admin index or handle as needed
            TempData["ErrorMessage"] = "Không tìm thấy bài hát.";
            return RedirectToAction("Index", "Admin");
        }



    }
}