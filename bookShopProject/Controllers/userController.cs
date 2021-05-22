using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bookShopProject.Models;
namespace bookShopProject.Controllers
{
    public class userController : Controller
    {
        private bookShopEntities _db = new bookShopEntities();
        // GET: user
        public ActionResult Index()
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("~/user/Login");
            }
            return View(_db.User.ToList());
        }

        // GET: user/Details/5
        public ActionResult Details(int id)
        {
            var userDetails = _db.userDetails.Find(id);
            return View(userDetails);
        }

        // GET: user/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: user/Create
        [HttpPost]
        public ActionResult Create(ViewModel.userViewModel obj)
        {
           
            userDetails ud = new userDetails();
            ud.name = obj.name;
            ud.surname = obj.surname;
            ud.postCode = obj.postCode;
            ud.street = obj.street;
            ud.houseNumber = obj.houseNumber;
            ud.phoneNumber = obj.phoneNumber;
            _db.userDetails.Add(ud);
            _db.SaveChanges();
            User u = new User();
            u.username = obj.username;
            u.password = obj.password;
            u.Role = obj.Role;
            u.userDetails_id = ud.id;
            _db.User.Add(u);
            _db.SaveChanges();

            return RedirectToAction("Index");
        } 

        // GET: user/Edit/5
        public ActionResult Edit(int id)
        {
            var userToEdit = _db.User.SingleOrDefault(m => m.id == id); ;
            return View(userToEdit);
        }

        // POST: user/Edit/5
        [HttpPost]
        public ActionResult Edit(User userToEdit)
        {
            var originalUser = _db.User.SingleOrDefault(m => m.id == userToEdit.id);
            try
            {
                // TODO: Add update logic here
                if (TryUpdateModel(originalUser, new string[] {
                    "username", "password", "Role","userDetails_id" }))
                {
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(originalUser);
            }
        }

        // GET: user/Delete/5
        public ActionResult Delete(int id)
        {
            var userToDelete = _db.User.SingleOrDefault(m => m.id == id);
            return View(userToDelete);
        }

        // POST: user/Delete/5
        [HttpPost]
        public ActionResult Delete(User userToDelete)
        {
            try
            {
                // TODO: Add delete logic here
                var deleteUser = _db.User.SingleOrDefault(m => m.id == userToDelete.id);
                var deleteUserDetails = _db.userDetails.SingleOrDefault(m => m.id == userToDelete.userDetails_id);
                if (!ModelState.IsValid)
                    return RedirectToAction("Index");
                _db.User.Remove(deleteUser);
                _db.SaveChanges();
                _db.userDetails.Remove(deleteUserDetails);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult LogIn()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public ActionResult Authorize(User userModel)
        {
            using (_db)
            {
                var userInfo = _db.User.Where(x => x.username == userModel.username && x.password == userModel.password).FirstOrDefault();
                if (userInfo == null)
                {
                    userModel.LoginErrorMessage = "wrong username or password";
                    return View("LogIn", userModel);
                }
                else
                {
                    Session["userId"] = userInfo.id;
                    return RedirectToAction("Index", "Home");
                }
              
            }
        }
    }
}
