using CTURectruits.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTURectruits.ViewModels;

namespace CTURectruits.Controllers
{
    public class HomeController : Controller
    {
        private CTURecruitsEntities db = new CTURecruitsEntities();
        
        #region Index

        public ActionResult Index()
        {
            var model = new HomeVM();
            return View(model);
        }

        #endregion

        #region Login

        //Login is better managed with an account controller which 
        //can authenticate users based on their identity.
        //Remember this for future!
        //Do not store passwords as plain text.

        public ActionResult Login()
        {
            var model = new LoginVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "Email,Password")] User user)
        {
            var model = new LoginVM();

            if (ModelState.IsValid)
            {
                //Check if the user exists and if his credentials match 
                //what has been entered

                
                foreach (var User in db.Users)
                {
                    if (User.Email == user.Email && User.Password == user.Password)
                    {
                        model.CurrentUserInstance = User;

                        return View("Index", new HomeVM());
                    }
                }
            }

            
            return RedirectToAction("Login");
        }

        #endregion

        #region Logout
        public ActionResult Logout()
        {
            //Set the Current User back to null 
            var model = new LoginVM();
            model.CurrentUserInstance = null;

            return View("Index", new HomeVM());
        }

        #endregion
        

    }
}