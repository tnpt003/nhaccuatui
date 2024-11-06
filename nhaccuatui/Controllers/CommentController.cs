using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        [HttpPost]
        public ActionResult AddComment(int userId, int songId, string commentText)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Add the comment to the Comments table
            string sql = $"INSERT INTO Comments (UserID, SongID, CommentText) VALUES ({userId}, {songId}, '{commentText}')";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeleteComment(int commentId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Delete the comment based on CommentID
            string sql = $"DELETE FROM Comments WHERE CommentID = {commentId}";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public ActionResult UpdateComment(int commentId, int userId, int songId, string commentText)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Update the comment record in the Comments table
            string sql = $"UPDATE Comments SET UserID = {userId}, SongID = {songId}, CommentText = '{commentText}' WHERE CommentID = {commentId}";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult GetCommentDetails(int commentId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Retrieve the comment details
            string sql = $"SELECT * FROM Comments WHERE CommentID = {commentId}";
            var commentDetails = db.get(sql);

            // Populate the user and song data for the dropdowns
            ViewBag.listUs = db.get("SELECT * FROM Users ORDER BY USERID DESC");
            ViewBag.list = db.get("SELECT * FROM Songs ORDER BY SONGID DESC");

            if (commentDetails != null && commentDetails.Count > 0)
            {
                ViewBag.CommentData = commentDetails[0];  // Assuming commentDetails[0] contains the data you need
                return View("~/Views/Admin/UpdateComment.cshtml"); // Specify the correct view path
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}
