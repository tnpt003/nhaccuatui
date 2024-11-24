using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using nhaccuatui.Models;

namespace nhaccuatui.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(string title = null)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            ViewBag.SearchTerm = title;

            if (string.IsNullOrEmpty(title))
            {
                ViewBag.list = db.get("SELECT * FROM Songs ORDER BY SONGID DESC");
                ViewBag.listUs = db.get("SELECT * FROM Users ORDER BY USERID DESC");
                ViewBag.listAr = db.get("SELECT * FROM Artists ORDER BY ARTISTID DESC");
                ViewBag.listAl = db.get("SELECT * FROM Albums ORDER BY ALBUMID DESC");
                ViewBag.listGe = db.get("SELECT * FROM Genres ORDER BY GENREID DESC");
                ViewBag.listPl = db.get("SELECT * FROM Playlists ORDER BY PLAYLISTID DESC");
                ViewBag.listPlS = db.get(@"
    SELECT ps.PlaylistID, pl.Name AS PlaylistName, ps.SongID, s.Title AS SongTitle, ps.AddedDate
    FROM PlaylistSongs ps
    JOIN Playlists pl ON ps.PlaylistID = pl.PlaylistID
    JOIN Songs s ON ps.SongID = s.SongID
    ORDER BY ps.PlaylistID DESC
    ");

                ViewBag.listLi = db.get(@"SELECT 
    L.LikeID,
    U.UserName,
    S.Title AS SongTitle,
    L.LikedDate
    FROM Likes L
    JOIN Users U ON L.UserID = U.UserID
    JOIN Songs S ON L.SongID = S.SongID
    ORDER BY L.LikeID DESC;
    ");
                ViewBag.listCo = db.get(@"
        SELECT c.CommentID, u.UserName, s.Title AS SongTitle, c.CommentText, c.CommentDate
        FROM Comments c
        JOIN Users u ON c.UserID = u.UserID
        JOIN Songs s ON c.SongID = s.SongID
        ORDER BY c.CommentID DESC
    ");
            }
            else
            {
                // Search for albums based on the search term
                ViewBag.ListAl = db.get($@"
        SELECT * 
        FROM Albums 
        WHERE Title LIKE '%{title}%' 
        ORDER BY ALBUMID DESC");

                // Search for songs based on the search term
                ViewBag.list = db.get($@"
        SELECT * 
        FROM Songs 
        WHERE Title LIKE '%{title}%' 
        ORDER BY SONGID DESC");

                // Search for users based on the search term
                ViewBag.listUs = db.get($@"
        SELECT * 
        FROM Users 
        WHERE Username LIKE '%{title}%' OR Email LIKE '%{title}%' 
        ORDER BY USERID DESC");

                // Search for artists based on the search term
                ViewBag.listAr = db.get($@"
        SELECT * 
        FROM Artists 
        WHERE Name LIKE '%{title}%' OR Bio LIKE '%{title}%' 
        ORDER BY ARTISTID DESC");

                // Search for genres based on the search term
                ViewBag.listGe = db.get($@"
        SELECT * 
        FROM Genres 
        WHERE Name LIKE '%{title}%' 
        ORDER BY GENREID DESC");

                // Search for playlists based on the search term
                ViewBag.listPl = db.get($@"
        SELECT * 
        FROM Playlists 
        WHERE Name LIKE '%{title}%' 
        ORDER BY PLAYLISTID DESC");

                // Search for playlist songs based on the search term
                ViewBag.listPlS = db.get($@"
        SELECT ps.PlaylistID, pl.Name AS PlaylistName, ps.SongID, s.Title AS SongTitle, ps.AddedDate
        FROM PlaylistSongs ps
        JOIN Playlists pl ON ps.PlaylistID = pl.PlaylistID
        JOIN Songs s ON ps.SongID = s.SongID
        WHERE pl.Name LIKE '%{title}%' OR s.Title LIKE '%{title}%'
        ORDER BY ps.PlaylistID DESC");

                // Search for likes based on the search term
                ViewBag.listLi = db.get($@"
        SELECT 
        L.LikeID,
        U.UserName,
        S.Title AS SongTitle,
        L.LikedDate
        FROM Likes L
        JOIN Users U ON L.UserID = U.UserID
        JOIN Songs S ON L.SongID = S.SongID
        WHERE U.UserName LIKE '%{title}%' OR S.Title LIKE '%{title}%'
        ORDER BY L.LikeID DESC");

                // Search for comments based on the search term
                ViewBag.listCo = db.get($@"
        SELECT 
        c.CommentID,
        u.UserName,
        s.Title AS SongTitle,
        c.CommentText,
        c.CommentDate
        FROM Comments c
        JOIN Users u ON c.UserID = u.UserID
        JOIN Songs s ON c.SongID = s.SongID
        WHERE u.UserName LIKE '%{title}%' OR s.Title LIKE '%{title}%' OR c.CommentText LIKE '%{title}%'
        ORDER BY c.CommentID DESC");
            }

            return View();
        }
    }
}