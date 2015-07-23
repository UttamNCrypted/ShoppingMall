using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingMall.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        //
        // GET: /Admin/AdminHome/
        private ShoppingMall.Models.ShoppingCartDatabaseContexts db = new ShoppingMall.Models.ShoppingCartDatabaseContexts();
        public ActionResult Index()
        {
            if (Session["AdminUserFirstName"] == null)
                return RedirectToAction("Login");
            else
                return View();
            
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string EmailID, string Password)
        {
            try
            {
                var user = db.Users.Where(u => u.EmailID == EmailID && u.Password == Password && u.IsAdmin == true).ToList();

                if (user.Count > 0)
                {

                    Session["UserName"] = user[0].EmailID;
                    Session["AdminUserFirstName"] = user[0].FristName + " " + user[0].LastName;
                    return RedirectToAction("Index","AdminHome");

                }
                else
                {
                    ViewBag.ErrorMsg = "Email or Password Incorrect, Please try again";
                    return View();
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Logout()
        {
            
            Session["AdminUserFirstName"] = null;
            return RedirectToAction("Index", "AdminHome");
        }    
    }
}
