using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private readonly NhaccuatuiModel db = new NhaccuatuiModel();

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