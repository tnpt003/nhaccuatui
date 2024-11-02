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
        [HttpPost]
        public ActionResult UpdateArtist(int artistId, string name, string country, string bio)
        {
            try
            {
                // Instantiate the database model
                NhaccuatuiModel db = new NhaccuatuiModel();

                // Directly update artist data in the database
                db.get($"UPDATE Artists SET Name = N'{name}', Country = N'{country}', Bio = N'{bio}' WHERE ArtistID = {artistId}");
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
            }

            // Redirect to the Index action in the Admin controller after updating the artist
            return RedirectToAction("Index", "Admin");
        }
        public ActionResult GetArtistUpdate(int id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Direct SQL query to retrieve artist data based on the ArtistID
            var artistData = db.get($"SELECT * FROM Artists WHERE ArtistID = {id}");

            if (artistData != null && artistData.Count > 0)
            {
                ViewBag.ArtistData = artistData[0]; // Assuming artistData contains a single row
                return View("~/Views/Admin/UpdateArtist.cshtml");
            }

            // If no artist found, redirect back to the admin index or handle as needed
            TempData["ErrorMessage"] = "Không tìm thấy nghệ sĩ.";
            return RedirectToAction("Index", "Admin");
        }

    }
}