using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using nhaccuatui.Models;


namespace nhaccuatui.Controllers
{
    public class LoginRegisterController : Controller
    {
        // GET: LoginRegister
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XulyDangNhap(string username, string password)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Modify the query to match username and password
            var query = $"SELECT * FROM Users WHERE Username = '{username}' AND PasswordHash = '{password}'";
            var result = db.get(query); // result is an ArrayList where each element is an ArrayList representing a row

            if (result != null && result.Count > 0)
            {
                // Assuming the first element is the row with user details
                var userRow = result[0] as ArrayList;

                if (userRow != null)
                {
                    // Accessing the UserID and Username from the row (index 0 for UserID, index 1 for Username)
                    var userID = userRow[0].ToString();  // Assuming UserID is at index 0
                    var retrievedUsername = userRow[1].ToString(); // Use a different name for the username variable

                    // Store the UserID and Username in session
                    Session["UserID"] = userID;
                    Session["Username"] = retrievedUsername;

                    return RedirectToAction("Index", "Home"); // Redirect to the desired page on success
                }
            }

            // If no user is found or login fails, show an error message
            TempData["LoginError"] = "Sai tên đăng nhập hoặc mật khẩu"; // Set error message
            return RedirectToAction("Index", "LoginRegister"); // Redirect back to login page
        }

        [HttpPost]
        public ActionResult XulyDangKy(string username, string email, string password, string confirmPassword)
        {
            NhaccuatuiModel db = new NhaccuatuiModel();

            // Check if passwords match
            if (password != confirmPassword)
            {
                TempData["RegisterError"] = "Mật khẩu không khớp";
                return RedirectToAction("Index", "LoginRegister");
            }

            // Check if username or email already exists
            var existingUserCheck = db.get($"SELECT * FROM Users WHERE Username = '{username}' OR Email = '{email}'");
            if (existingUserCheck.Count > 0)
            {
                TempData["RegisterError"] = "Tên đăng nhập hoặc email đã tồn tại";
                return RedirectToAction("Index", "LoginRegister");
            }

            // Store the password directly (Note: this is not secure for production use)
            string sql = $"INSERT INTO Users (Username, Email, PasswordHash, DateJoined) VALUES ('{username}', '{email}', '{password}', GETDATE())";
            db.get(sql);

            TempData["RegisterSuccess"] = "Đăng ký thành công. Vui lòng đăng nhập.";
            return RedirectToAction("Index", "LoginRegister");
        }
        public ActionResult Logout()
        {
            Session["taikhoan"] = null; // Clear the session on logout
            return RedirectToAction("Index", "Home"); // Redirect to home or login page
        }

    }
}