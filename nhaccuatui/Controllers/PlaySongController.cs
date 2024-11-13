using System;
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
            s.AlbumID, 
            s.ReleaseDate,
            a.Name AS ArtistName, 
            COUNT(l.LikeID) AS LikeCount 
        FROM Songs s
        LEFT JOIN Likes l ON s.SongID = l.SongID
        LEFT JOIN Artists a ON a.ArtistID = (SELECT ArtistID FROM Albums WHERE AlbumID = s.AlbumID)
        GROUP BY s.SongID, s.Title, s.FileUrl, s.AlbumID, s.ReleaseDate, a.Name
        ORDER BY LikeCount DESC");

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

    }
}
