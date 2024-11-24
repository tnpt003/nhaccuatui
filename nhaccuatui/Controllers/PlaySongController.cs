using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nhaccuatui.Models;

namespace nhaccuatui.Controllers
{
    public class PlaySongController : Controller
    {

        // GET: Playlists
        public ActionResult Core(int id)
        {
            NhaccuatuiModel db = new NhaccuatuiModel(); // Reuse DbContext
            // Fetch album data based on the albumId
            var albumData = db.get($"SELECT * FROM Albums WHERE AlbumID = {id}");
            if (albumData == null || albumData.Count == 0)
            {
                TempData["ErrorMessage"] = "Không tìm thấy album.";
                return RedirectToAction("Index", "Home");
            }

            // Fetch songs related to the album
            var songsData = db.get($@"
        SELECT s.SongID, s.Title, s.Duration, s.FileUrl, a.Name AS ArtistName 
        FROM Songs s
        LEFT JOIN Albums al ON s.AlbumID = al.AlbumID
        LEFT JOIN Artists a ON a.ArtistID = (SELECT ArtistID FROM Albums WHERE AlbumID = s.AlbumID)
        WHERE s.AlbumID = {id}
    ");

            // SQL query to get top 5 songs by like count
            ViewBag.rank1Song = db.get(@"
        SELECT TOP 1 
            s.SongID, 
            s.Title, 
            s.FileUrl, 
            s.ImageUrl, -- Added ImageUrl
            s.AlbumID, 
            s.ReleaseDate,
            a.Name AS ArtistName, 
            COUNT(l.LikeID) AS LikeCount 
        FROM Songs s
        LEFT JOIN Likes l ON s.SongID = l.SongID
        LEFT JOIN Artists a ON a.ArtistID = (SELECT ArtistID FROM Albums WHERE AlbumID = s.AlbumID)
        GROUP BY 
            s.SongID, 
            s.Title, 
            s.FileUrl, 
            s.ImageUrl, -- Added ImageUrl to GROUP BY
            s.AlbumID, 
            s.ReleaseDate, 
            a.Name
        ORDER BY LikeCount DESC
        ");

            ViewBag.rank2AndAboveSongs = db.get(@"
        SELECT 
            s.SongID, 
            s.Title, 
            s.FileUrl, 
            s.AlbumID, 
            s.ReleaseDate,
            a.Name AS ArtistName, 
            COUNT(l.LikeID) AS LikeCount 
        FROM Songs s
        LEFT JOIN Likes l ON s.SongID = l.SongID
        LEFT JOIN Artists a ON a.ArtistID = (SELECT ArtistID FROM Albums WHERE AlbumID = s.AlbumID)
        GROUP BY s.SongID, s.Title, s.FileUrl, s.AlbumID, s.ReleaseDate, a.Name
        ORDER BY LikeCount DESC
        OFFSET 1 ROWS FETCH NEXT 9 ROWS ONLY");

            // Pass the updated album data, songs, and ranking data to the view
            ViewBag.AlbumData = albumData[0];
            ViewBag.SongsData = songsData;
            return View("~/Views/PlaySong/Core.cshtml");
        }
        public ActionResult PlaySong(int id)
        {
            // Initialize the database model
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Query to get the song details
            string songQuery = @"
    SELECT s.SongID, s.Title AS SongTitle, s.FileUrl, s.ImageUrl, a.Title AS AlbumTitle, 
           g.Name AS GenreName, ar.Name AS ArtistName
    FROM Songs s
    JOIN Albums a ON s.AlbumID = a.AlbumID
    JOIN Genres g ON s.GenreID = g.GenreID
    JOIN Artists ar ON a.ArtistID = ar.ArtistID
    WHERE s.SongID = " + id;

            var songDetails = db.get(songQuery);
            ViewBag.SongDetails = songDetails.Count > 0 ? songDetails[0] : null;

            // Query to fetch the top 5 random albums
            string playlistQuery = @"
    SELECT TOP 5 a.AlbumID, a.Title AS AlbumTitle, a.CoverImageUrl, ar.Name AS ArtistName
    FROM Albums a
    JOIN Artists ar ON a.ArtistID = ar.ArtistID
    ORDER BY NEWID()";

            var playlist = db.get(playlistQuery);
            ViewBag.Playlist = playlist;

            // Query to fetch related songs
            string relatedSongsQuery = @"
    SELECT s.SongID, s.Title AS SongTitle, ar.Name AS ArtistName
    FROM Songs s
    JOIN Albums a ON s.AlbumID = a.AlbumID
    JOIN Artists ar ON a.ArtistID = ar.ArtistID
    WHERE s.GenreID = (SELECT GenreID FROM Songs WHERE SongID = " + id + ") AND s.SongID != " + id;

            var relatedSongs = db.get(relatedSongsQuery);
            ViewBag.RelatedSongs = relatedSongs;

            // Query to check if the user has liked the song
            var userId = Convert.ToInt32(Session["UserID"]); // Ensure correct user ID from session

            var likeQuery = $@"
    SELECT COUNT(*) 
    FROM Likes 
    WHERE UserID = {userId} AND SongID = {id}";

            var likeCount = db.get(likeQuery); // Execute the query

            bool isLiked = false;
            if (likeCount != null && likeCount.Count > 0)
            {
                var countStr = likeCount[0].ToString();
                if (int.TryParse(countStr, out int count))
                {
                    isLiked = count > 0;
                }
            }

            // Pass the 'isLiked' value to the view
            ViewBag.IsLiked = isLiked;

            return View("PlaySong");
        }

        [HttpPost]
        public JsonResult AddToPlaylist(int songId, int playlistId)
        {
            try
            {
                NhaccuatuiModel db = new NhaccuatuiModel();

                // Check if the song is already in the playlist
                var checkQuery = $@"
            SELECT * 
            FROM PlaylistSongs 
            WHERE PlaylistID = {playlistId} AND SongID = {songId}";
                var checkResult = db.get(checkQuery);

                if (checkResult != null && checkResult.Count > 0)
                {
                    return Json(new { success = false, message = "Song already in playlist" });
                }

                // Insert the song into the playlist
                var insertQuery = $@"
            INSERT INTO PlaylistSongs (PlaylistID, SongID) 
            VALUES ({playlistId}, {songId})";
                db.get(insertQuery);

                return Json(new { success = true, message = "Song added to playlist" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }


        public JsonResult GetUserPlaylists()
        {
            var userId = Session["UserID"].ToString();
            NhaccuatuiModel db = new NhaccuatuiModel();

            var query = $@"
            SELECT PlaylistID, Name 
            FROM Playlists 
            WHERE UserID = {userId}";
            var result = db.get(query);

            var playlists = result.Cast<ArrayList>().Select(row => new
            {
                PlaylistID = row[0].ToString(),
                Name = row[1].ToString()
            }).ToList();

            return Json(new { success = true, playlists }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ToggleLike(int songId)
        {
            try
            {
                int userId = Convert.ToInt32(Session["UserID"]); // Get the current user ID
                NhaccuatuiModel db = new NhaccuatuiModel();

                var checkQuery = $@"
        SELECT * 
        FROM Likes 
        WHERE UserID = {userId} AND SongID = {songId}";

                var checkResult = db.get(checkQuery); // Check if the user has liked the song

                bool isLiked = false;
                if (checkResult != null && checkResult.Count > 0)
                {
                    var deleteQuery = $@"
            DELETE FROM Likes 
            WHERE UserID = {userId} AND SongID = {songId}";
                    db.get(deleteQuery); // Execute delete query

                    isLiked = false; // Unliked the song
                }
                else
                {
                    var insertQuery = $@"
            INSERT INTO Likes (UserID, SongID, LikedDate) 
            VALUES ({userId}, {songId}, GETDATE())";
                    db.get(insertQuery); // Insert like into the database

                    isLiked = true; // Liked the song
                }

                // Return the updated like state
                return Json(new { success = true, isLiked = isLiked });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }




    }
}
