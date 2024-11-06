using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class LikeController : Controller
    {
        // GET: Like
        [HttpPost]
        public ActionResult AddLike(int userId, int songId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Add the like to the Likes table
            string sql = $"INSERT INTO Likes (UserID, SongID) VALUES ({userId}, {songId})";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }
        public ActionResult DeleteLike(int likeId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Delete the like based on LikeID
            string sql = $"DELETE FROM Likes WHERE LikeID = {likeId}";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }
        [HttpPost]
        public ActionResult UpdateLike(int likeId, int userId, int songId, int? oldSongId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Handle the case where the oldSongId is null (optional parameter)
            if (oldSongId.HasValue)
            {
                // You can use oldSongId for logging or further checks
                // For example, compare oldSongId with songId before updating
            }

            // Update the like record in the Likes table
            string sql = $"UPDATE Likes SET UserID = {userId}, SongID = {songId} WHERE LikeID = {likeId}";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult GetLikeDetails(int likeId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Retrieve the like details
            string sql = $"SELECT * FROM Likes WHERE LikeID = {likeId}";
            var likeDetails = db.get(sql);

            // Populate the user and song data for the dropdowns
            ViewBag.listUs = db.get("SELECT * FROM Users ORDER BY USERID DESC");
            ViewBag.list = db.get("SELECT * FROM Songs ORDER BY SONGID DESC");

            if (likeDetails != null && likeDetails.Count > 0)
            {
                ViewBag.LikeData = likeDetails[0];  // Assuming likeDetails[0] contains the data you need
                return View("~/Views/Admin/UpdateLike.cshtml"); // Specify the correct view path
            }
            return RedirectToAction("Index", "Admin");
        }

    }
}