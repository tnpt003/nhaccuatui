using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class GenreController : Controller
    {
        // GET: Genre 

        public ActionResult AddGenre(string name)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Add new genre
            db.get($"EXEC AddGenre @Name = '{name}'");

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeleteGenre(int genreId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Delete all songs in this genre
            db.get($"DELETE FROM Songs WHERE GenreID = {genreId}");

            // Finally, delete the genre
            db.get($"DELETE FROM Genres WHERE GenreID = {genreId}");

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult GetGenreUpdate(int id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            // Direct SQL query to retrieve genre data based on the GenreID
            var genreData = db.get($"SELECT * FROM Genres WHERE GenreID = {id}");

            if (genreData != null && genreData.Count > 0)
            {
                ViewBag.GenreData = genreData[0]; // Assuming genreData contains a single row
                return View("~/Views/Admin/UpdateGenre.cshtml");
            }

            // If no genre found, redirect back to the admin index or handle as needed
            TempData["ErrorMessage"] = "Không tìm thấy thể loại.";
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult UpdateGenre(int genreId, string name)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            try
            {
                db.get($"EXEC UpdateGenre {genreId}, N'{name}';");
            }
            catch (Exception ex)
            {
                // Log or handle exception if needed
            }

            // Redirect to the Index action in the Admin controller after updating the genre
            return RedirectToAction("Index", "Admin");
        }
    }

}