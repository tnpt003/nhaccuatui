using System;
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
            // Pass the result to the view
            ViewBag.listRandomAlbums1 = db.get("SELECT TOP 5 * FROM Albums ORDER BY NEWID()");
            // Pass the result to the view
            ViewBag.listRandomAlbums2 = db.get("SELECT TOP 5 * FROM Albums ORDER BY NEWID()");
            // Pass the result to the view
            ViewBag.listRandomAlbums3 = db.get("SELECT TOP 5 * FROM Albums ORDER BY NEWID()");
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

            return View();
        }
        // Search Action
        public ActionResult SearchSongsByTitle(string title)
        {
            // Initialize the database model
            NhaccuatuiModel db = new NhaccuatuiModel();

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


            // Add wildcards for LIKE queries
            string searchTitle = "%" + title + "%";

            // Pass the search title to the ViewBag
            ViewBag.searchTitle = title;  // Set the search title in ViewBag

            // Execute the query to get songs
            ViewBag.listSo = db.get($@"
        SELECT 
            s.SongID, 
            s.AlbumID, 
            s.Title AS SongTitle, 
            s.ReleaseDate, 
            a.Title AS AlbumTitle, 
            s.ImageUrl AS SongImageUrl
        FROM 
            Songs s
        LEFT JOIN 
            Albums a ON s.AlbumID = a.AlbumID
        WHERE 
            s.Title LIKE '{searchTitle}'
    ");

            // Execute the query to get albums
            ViewBag.listAl = db.get($@"
        SELECT 
            a.AlbumID, 
            a.ReleaseDate, 
            a.Title AS AlbumTitle, 
            ar.Name AS ArtistName, 
            a.CoverImageUrl AS AlbumCoverImageUrl
        FROM 
            Albums a
        LEFT JOIN 
            Artists ar ON a.ArtistID = ar.ArtistID
        WHERE 
            a.Title LIKE '{searchTitle}'
    ");

            // Execute the query to get artists
            ViewBag.listAr = db.get($@"
        SELECT 
            ar.ArtistID, 
            ar.Name AS ArtistName, 
            ar.Bio, 
            ar.Country
        FROM 
            Artists ar
        WHERE 
            ar.Name LIKE '{searchTitle}'
    ");
            // Return the SearchResults view
            return View("SearchResults");
        }

    }
}