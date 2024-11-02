using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class AlbumController : Controller
    {
        // GET: Album
        public ActionResult DeleteAlbum(int id)
        {
            try
            {
                NhaccuatuiModel db = new NhaccuatuiModel();
                db.get($"DELETE FROM Albums WHERE AlbumID = {id}");
            }
            catch (Exception ex)
            {
                // Handle any errors if needed
            }

            return RedirectToAction("Index", "Admin");
        }
        public ActionResult GetAlbumUpdate(int id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            var albumData = db.get($"SELECT * FROM Albums WHERE AlbumID = {id}");

            if (albumData != null && albumData.Count > 0)
            {
                ViewBag.AlbumData = albumData[0];
                ViewBag.ListArtists = db.get("SELECT * FROM Artists ORDER BY ArtistID DESC");
                return View("~/Views/Admin/UpdateAlbum.cshtml");
            }

            TempData["ErrorMessage"] = "Không tìm thấy album.";
            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public ActionResult UpdateAlbum(int albumId, int artistId, string title, DateTime releaseDate, HttpPostedFileBase coverImage)
        {
            try
            {
                string coverImageFilename = null;

                // Process cover image file upload
                if (coverImage != null && coverImage.ContentLength > 0)
                {
                    coverImageFilename = Path.GetFileName(coverImage.FileName);
                    string imagePath = Path.Combine(Server.MapPath("~/Image/Data"), coverImageFilename);
                    coverImage.SaveAs(imagePath);
                }

                NhaccuatuiModel db = new NhaccuatuiModel();

                // Update album in database with or without a new cover image
                db.get($"UPDATE Albums SET ArtistID = {artistId}, Title = N'{title}', ReleaseDate = '{releaseDate}', " +
                       $"CoverImageUrl = {(coverImageFilename != null ? $"'{coverImageFilename}'" : "CoverImageUrl")} WHERE AlbumID = {albumId}");
            }
            catch (Exception ex)
            {
                // Handle the error if needed
            }

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult AddAlbum(int artistId, string title, DateTime releaseDate, HttpPostedFileBase coverImage)
        {
            try
            {
                string coverImageFilename = null;

                // Process cover image file upload
                if (coverImage != null && coverImage.ContentLength > 0)
                {
                    coverImageFilename = Path.GetFileName(coverImage.FileName);
                    string imagePath = Path.Combine(Server.MapPath("~/Image/Data"), coverImageFilename);
                    coverImage.SaveAs(imagePath);
                }

                // Save album data in the database
                NhaccuatuiModel db = new NhaccuatuiModel();
                db.get($"INSERT INTO Albums (ArtistID, Title, ReleaseDate, CoverImageUrl) VALUES ({artistId}, N'{title}', '{releaseDate}', '{coverImageFilename}')");
            }
            catch (Exception ex)
            {
                // Handle any errors if needed
            }

            return RedirectToAction("Index", "Admin");
        }

    }
}