using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTURectruits.Models;
using CTURectruits.ViewModels;

namespace CTURectruits.Controllers
{
    public class UsersController : Controller
    {
        private CTURecruitsEntities db = new CTURecruitsEntities();

        #region Employ

        // GET: Users
        public async Task<ActionResult> Employ()
        {
            var model = new EmployVM();

            //Pass a list of users to the view via the model.
            model.Users = await db.Users.Include(u => u.UserType).ToListAsync();

            return View(model);
        }

        #endregion

        #region Success

        public ActionResult Success()
        {
            var model = new SuccessVM();

            //Pass a list of users who have found success to the view via the model.

            model.successUsers = db.Users.Where(u => u.FoundDreamJob == true ).ToList();

            return View(model);
        }



        #endregion

        #region ViewUser

        // GET: Users/Details/5
        public async Task<ActionResult> ViewUser(int? id)
        {
            var model = new ViewUserVM();

            //Account for the fact that the id could be null
            if (id == null)
            {
                id = model.CurrentUserInstance.UserId;

            }

            //Get the user with the requested UserID
            model.userToView = await db.Users.FindAsync(id);

            //Handle failure to find the user
            if (model.userToView == null)
            {
                return HttpNotFound();
            }
            
            return View(model);
        }

        #endregion

        #region Register
        // GET: Users/Create
        public ActionResult Register()
        {
            var model = new RegisterVM();

            //Create a temporary user than can be passed to the UI and  
            //used to store the data that needs to be posted to the database
            model.tempUser = new User
            {
                Role = 1,
                ShowCV = true

            };
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeText");
            return View(model);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "tempUser")] RegisterVM regVM, int UserTypeId)
        {
            //Take in the data that the user has entered in a temporary object and write it to the database


            regVM.tempUser.UserTypeId = UserTypeId;

            var model = new RegisterVM();
            
            if (ModelState.IsValid)
            {
                //Use the temporary user with populated data, to save the data to the database
                db.Users.Add(regVM.tempUser);
                await db.SaveChangesAsync();

                //Save the info of the user that was just created into and object in our model
                model.createdUser = regVM.tempUser;

                return RedirectToAction("Login","Home");
            }
            //Set the user that is being veiwed to the new user which has just been created.
            model.tempUser = regVM.tempUser;
            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeText", regVM.tempUser.UserTypeId);
            return View(model);
        }
        #endregion

        #region Edit
        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var model = new EditVM();

            if (id == null)
            {
                id = model.CurrentUserInstance.UserId;
            }

            model.userToEdit = await db.Users.FindAsync(id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeText", model.userToEdit.UserTypeId);
            return View(model);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind(Include = "UserId,Name,Email,Password,ShowCV,CV,Role,YearsXP,Area,UserTypeId")] User user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include ="userToEdit")] EditVM editVM, int UserTypeId)
        {
            editVM.userToEdit.UserTypeId = UserTypeId;

            var model = new EditVM();

            if (ModelState.IsValid)
            {
                db.Entry(editVM.userToEdit).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (editVM.userToEdit.UserId == model.CurrentUserInstance.UserId)
                {
                    model.CurrentUserInstance = editVM.userToEdit;
                    model.editedUser = editVM.userToEdit;

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Employ", "Users");
                }
            }

            ViewBag.UserTypeId = new SelectList(db.UserTypes, "UserTypeId", "UserTypeText", editVM.userToEdit.UserTypeId);
            return View(model);
        }
        #endregion

        #region Remove

        //Users/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Employ");
        }


        #endregion

        #region Search
        [HttpPost]
        public ActionResult Search([Bind(Include = "keywordFilter,yearsFilter")] EmployVM emVM)
        {
            EmployVM model = new EmployVM();

            if ((emVM.keywordFilter != null && emVM.keywordFilter != "") && emVM.yearsFilter != 0)
            {
                //Both values filled
                model.Users = (db.Users.Where(u => u.YearsXP >= emVM.yearsFilter)).Where(u => u.CV.Contains(emVM.keywordFilter)).ToList();
            }
            else if (emVM.yearsFilter == 0 && (emVM.keywordFilter != null && emVM.keywordFilter != ""))
            {
                //keyword filled
                model.Users = db.Users.Where(u => u.CV.Contains(emVM.keywordFilter)).ToList();
            }
            else if ((emVM.keywordFilter == null || emVM.keywordFilter == "") && emVM.yearsFilter != 0)
            {
                //years filled
                model.Users = db.Users.Where(u => u.YearsXP >= emVM.yearsFilter).ToList();
            }
            else
            {
                //both blank
                model.Users = db.Users.ToList();
            }

            return View("Employ", model);
        }
        #endregion

        #region dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
