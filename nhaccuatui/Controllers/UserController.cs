using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace nhaccuatui.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private readonly NhaccuatuiModel db = new NhaccuatuiModel();

        // Display user profile and playlists
        public ActionResult Index(int userId)
        {
            // Fetch user information
            string userQuery = $"SELECT * FROM Users WHERE UserID = {userId}";
            var userDetails = db.get(userQuery);

            if (userDetails != null && userDetails.Count > 0)
            {
                ViewBag.User = userDetails[0];
                ViewBag.UserID = userId;
            }
            else
            {
                TempData["Error"] = "Người dùng không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            // Fetch playlists for the user
            string playlistsQuery = $@"
    SELECT p.PlaylistID, p.Name, 
    (SELECT COUNT(*) FROM PlaylistSongs ps WHERE ps.PlaylistID = p.PlaylistID) AS SongCount
    FROM Playlists p WHERE p.UserID = {userId}";
            var playlists = db.get(playlistsQuery);
            ViewBag.Playlists = playlists;

            return View();
        }
        public ActionResult ViewPlaylist(int playlistId, int userId)
        {
            // Fetch playlist details
            string playlistDetailsQuery = $@"
    SELECT p.Name, COUNT(ps.SongID) AS SongCount
    FROM Playlists p
    LEFT JOIN PlaylistSongs ps ON p.PlaylistID = ps.PlaylistID
    WHERE p.PlaylistID = {playlistId}
    GROUP BY p.Name";
            var playlistDetails = db.get(playlistDetailsQuery);
            ViewBag.PlaylistDetails = playlistDetails.Count > 0 ? playlistDetails[0] : null;

            // Fetch songs in the playlist
            string songsQuery = $@"
    SELECT s.SongID, s.Title, s.Duration, s.ImageUrl, s.FileUrl
    FROM PlaylistSongs ps
    JOIN Songs s ON ps.SongID = s.SongID
    WHERE ps.PlaylistID = {playlistId}";
            var songs = db.get(songsQuery);
            ViewBag.Songs = songs;

            // Add UserID to ViewBag for potential further usage
            ViewBag.UserID = userId;

            return View();
        }


        [HttpPost]
        public ActionResult CreatePlaylist(int userId, string playlistName)
        {
            // SQL query to insert the new playlist
            string createPlaylistQuery = $@"
        INSERT INTO Playlists (UserID, Name, CreatedDate) 
        VALUES ({userId}, N'{playlistName}', GETDATE());
        SELECT SCOPE_IDENTITY();"; // Retrieve the newly created PlaylistID

            // Execute the query and get the result
            var result = db.get(createPlaylistQuery);

            if (result != null && result.Count > 0)
            {
                var playlistId = result[0].ToString(); // Get the ID of the new playlist (optional)
                TempData["SuccessMessage"] = "Playlist created successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create playlist.";
            }

            // Redirect back to the user's profile page
            return RedirectToAction("Index", new { userId });
        }

        [HttpPost]
        public ActionResult DeletePlaylist(int playlistId, int userId)
        {
            // Delete related playlist songs
            db.get($"DELETE FROM PlaylistSongs WHERE PlaylistID = {playlistId}");

            // Finally, delete the playlist
            db.get($"DELETE FROM Playlists WHERE PlaylistID = {playlistId}");

            // Redirect back to the user's profile
            return RedirectToAction("Index", new { userId });
        }


        // Admin
        // Add User
        [HttpPost]
        public ActionResult AddUser(string username, string email, string password)
        {
            string passwordHash = HashPassword(password); // Implement password hashing
            string sql = $"INSERT INTO Users (Username, Email, PasswordHash) VALUES ('{username}', '{email}', '{passwordHash}')";
            db.get(sql);
            return RedirectToAction("Index", "Admin");
        }

        // Show Update Form with User Data
        public ActionResult GetUserUpdate(int id)
        {
            var userData = db.get($"SELECT * FROM Users WHERE UserID = {id}");
            if (userData != null && userData.Count > 0)
            {
                ViewBag.UserData = userData[0];
                return View("~/Views/Admin/UpdateUser.cshtml");
            }
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("Index", "Admin");
        }

        // Update User
        [HttpPost]
        public ActionResult UpdateUser(int userId, string username, string email, string password)
        {
            string passwordHash = HashPassword(password); // Implement password hashing
            string sql = $"UPDATE Users SET Username = '{username}', Email = '{email}', PasswordHash = '{passwordHash}' WHERE UserID = {userId}";
            db.get(sql);
            return RedirectToAction("Index", "Admin");
        }

        // Delete User
        public ActionResult DeleteUser(int userId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Delete related comments, likes, and playlists first
            db.get($"DELETE FROM Comments WHERE UserID = {userId}");
            db.get($"DELETE FROM Likes WHERE UserID = {userId}");

            // Delete all songs in the user's playlists
            db.get($"DELETE FROM PlaylistSongs WHERE PlaylistID IN (SELECT PlaylistID FROM Playlists WHERE UserID = {userId})");

            // Delete playlists owned by the user
            db.get($"DELETE FROM Playlists WHERE UserID = {userId}");

            // Finally, delete the user
            db.get($"DELETE FROM Users WHERE UserID = {userId}");

            return RedirectToAction("Index", "Admin");
        }


        // Implement password hashing function
        private string HashPassword(string password)
        {
            // Use a hashing library like BCrypt or SHA256 for hashing passwords
            return password; // Replace with actual hashing logic
        }
    }
}