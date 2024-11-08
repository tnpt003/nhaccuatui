using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class PlaylistController : Controller
    {
        // GET: Playlist
        private readonly NhaccuatuiModel db = new NhaccuatuiModel();

        // Add Playlist
        [HttpPost]
        public ActionResult AddPlaylist(int userId, string name)
        {
            string sql = $"INSERT INTO Playlists (UserID, Name) VALUES ({userId}, '{name}')";
            db.get(sql);
            return RedirectToAction("Index", "Admin"); // Redirect as needed
        }

        // Show Update Form with Playlist Data
        public ActionResult GetPlaylistUpdate(int id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Assuming you have a method to get the playlist details
            var playlistData = db.get($"SELECT PlaylistID, UserID, Name FROM Playlists WHERE PlaylistID = {id}");

            if (playlistData != null && playlistData.Count > 0)
            {
                ViewBag.PlaylistData = playlistData; // Set the playlist data
                ViewBag.ListUs = db.get("SELECT UserID, Username FROM Users"); // Set the list of users
                return View("~/Views/Admin/UpdatePlaylist.cshtml");
            }

            TempData["ErrorMessage"] = "Không tìm thấy danh sách phát.";
            return RedirectToAction("Index", "Admin");
        }


        // Update Playlist
        [HttpPost]
        public ActionResult UpdatePlaylist(int playlistId, int userId, string name)
        {
            try
            {
                // Create the SQL statement for updating the playlist
                string sql = $"UPDATE Playlists SET UserID = {userId}, Name = '{name}' WHERE PlaylistID = {playlistId}";

                // Execute the update command
                db.get(sql);
            }
            catch (Exception ex)
            {
                // Optionally log the exception or handle it as needed
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi cập nhật playlist.";
            }

            // Redirect to the Index action in the Admin controller after updating the playlist
            return RedirectToAction("Index", "Admin");
        }

        // Delete Playlist
        public ActionResult DeletePlaylist(int playlistId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Delete related playlist songs
            db.get($"DELETE FROM PlaylistSongs WHERE PlaylistID = {playlistId}");

            // Finally, delete the playlist
            db.get($"DELETE FROM Playlists WHERE PlaylistID = {playlistId}");

            return RedirectToAction("Index", "Admin");
        }

    }
}