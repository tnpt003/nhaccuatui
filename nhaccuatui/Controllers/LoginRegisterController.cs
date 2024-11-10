using System;
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

            // Modify the query to match username and password (hashing recommended for security)
            var query = $"SELECT * FROM Users WHERE Username = '{username}' AND PasswordHash = '{password}'";
            var result = db.get(query);

            if (result.Count > 0)
            {
                // Store user data in session on successful login
                Session["taikhoan"] = result[0];
                return RedirectToAction("Index", "Home"); // Redirect to the desired page on success
            }
            else
            {
                TempData["LoginError"] = "Sai tên đăng nhập hoặc mật khẩu"; // Set error message on failure
                return RedirectToAction("Index", "LoginRegister"); // Redirect back to login page
            }
        }


    }
}