using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using nhaccuatui.Models;

namespace nhaccuatui.Controllers
{
    public class APIController : Controller
    {
        // GET: API
        public ActionResult Index()
        {
            // Initialize the database model
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Get data from each table
            var users = db.get("SELECT * FROM Users");
            var artists = db.get("SELECT * FROM Artists");
            var albums = db.get(@"
        SELECT 
            a.AlbumID, 
            a.Title, 
            a.ReleaseDate, 
            a.CoverImageUrl, 
            ar.Name AS ArtistName 
        FROM Albums a
        LEFT JOIN Artists ar ON a.ArtistID = ar.ArtistID
    ");
            var genres = db.get("SELECT * FROM Genres");
            var songs = db.get(@"
        SELECT 
            s.SongID, 
            s.Title, 
            s.ReleaseDate, 
            s.Duration, 
            s.ImageUrl, 
            s.FileUrl, 
            g.Name AS GenreName, 
            al.Title AS AlbumTitle, 
            ar.Name AS ArtistName 
        FROM Songs s
        LEFT JOIN Genres g ON s.GenreID = g.GenreID
        LEFT JOIN Albums al ON s.AlbumID = al.AlbumID
        LEFT JOIN Artists ar ON al.ArtistID = ar.ArtistID
    ");
            var playlists = db.get(@"
        SELECT 
            p.PlaylistID, 
            p.Name AS PlaylistName, 
            u.Username AS UserName, 
            p.CreatedDate 
        FROM Playlists p
        LEFT JOIN Users u ON p.UserID = u.UserID
    ");
            var playlistSongs = db.get(@"
        SELECT 
            ps.PlaylistID, 
            ps.SongID, 
            p.Name AS PlaylistName, 
            s.Title AS SongTitle 
        FROM PlaylistSongs ps
        LEFT JOIN Playlists p ON ps.PlaylistID = p.PlaylistID
        LEFT JOIN Songs s ON ps.SongID = s.SongID
    ");
            var likes = db.get(@"
        SELECT 
            l.LikeID, 
            u.Username AS UserName, 
            s.Title AS SongTitle, 
            l.LikedDate 
        FROM Likes l
        LEFT JOIN Users u ON l.UserID = u.UserID
        LEFT JOIN Songs s ON l.SongID = s.SongID
    ");
            var comments = db.get(@"
        SELECT 
            c.CommentID, 
            u.Username AS UserName, 
            s.Title AS SongTitle, 
            c.CommentText, 
            c.CommentDate 
        FROM Comments c
        LEFT JOIN Users u ON c.UserID = u.UserID
        LEFT JOIN Songs s ON c.SongID = s.SongID
    ");

            // Create a structured response
            var response = new
            {
                Users = users,
                Artists = artists,
                Albums = albums,
                Genres = genres,
                Songs = songs,
                Playlists = playlists,
                PlaylistSongs = playlistSongs,
                Likes = likes,
                Comments = comments
            };

            // Return as JSON
            return Json(response, JsonRequestBehavior.AllowGet);
        }

    }
}