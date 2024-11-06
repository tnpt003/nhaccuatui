using nhaccuatui.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nhaccuatui.Controllers
{
    public class PlaylistSongController : Controller
    {
        // GET: PlaylistSong
        [HttpPost]
        public ActionResult AddSongToPlaylist(int playlistId, int songId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            string sql = $"INSERT INTO PlaylistSongs (PlaylistID, SongID) VALUES ({playlistId}, {songId})";
            db.get(sql);

            return RedirectToAction("Index", "Admin"); // Adjust as needed
        }
        [HttpPost]
        public ActionResult UpdatePlaylistSong(int playlistId, int oldSongId, int newSongId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Update the song in the playlist
            string sql = $"UPDATE PlaylistSongs SET SongID = {newSongId} WHERE PlaylistID = {playlistId} AND SongID = {oldSongId}";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DeleteSongFromPlaylist(int playlistId, int songId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            string sql = $"DELETE FROM PlaylistSongs WHERE PlaylistID = {playlistId} AND SongID = {songId}";
            db.get(sql);

            return RedirectToAction("Index", "Admin");
        }
        // Show Update Form with Playlist Songs
        public ActionResult GetPlaylistSongUpdate(int playlistId)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Get songs currently in the playlist
            var playlistSongs = db.get($"SELECT ps.SongID, s.Title FROM PlaylistSongs ps JOIN Songs s ON ps.SongID = s.SongID WHERE ps.PlaylistID = {playlistId}");

            // Get all available songs
            var allSongs = db.get("SELECT SongID, Title FROM Songs");

            if (playlistSongs != null && playlistSongs.Count > 0)
            {
                ViewBag.PlaylistSongs = playlistSongs;
                ViewBag.AllSongs = allSongs;
                ViewBag.PlaylistID = playlistId;
                return View("~/Views/Admin/UpdatePlaylistSong.cshtml");
            }

            TempData["ErrorMessage"] = "Không tìm thấy bài hát trong danh sách phát.";
            return RedirectToAction("Index", "Admin");
        }

    }
}