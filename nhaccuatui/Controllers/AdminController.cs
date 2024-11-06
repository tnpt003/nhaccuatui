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
        public ActionResult Index()
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
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
            return View();
        }
    }
}