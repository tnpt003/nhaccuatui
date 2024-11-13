﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using nhaccuatui.Models;

namespace nhaccuatui.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            NhaccuatuiModel db = new NhaccuatuiModel();
            // Pass the result to the view
            ViewBag.listRandomAlbums = db.get("SELECT TOP 5 * FROM Albums ORDER BY NEWID()");
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

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        // Search Action
        public ActionResult SearchSongsByTitle(string title)
        {
            // Initialize the database model
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Use the EXEC command with the provided title parameter
            ViewBag.list = db.get($"EXEC SearchSongsByTitle '{title}'");

            // Return the SearchResults view
            return View("SearchResults");
        }

    }
}