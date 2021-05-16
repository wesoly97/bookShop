using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bookShopProject.Models;
namespace bookShopProject.Controllers
{
    public class BookController : Controller
    {
        private bookShopEntities _db = new bookShopEntities();
        // GET: book
        public ActionResult Index()
        {
            return View(_db.books.ToList());
        }

        // GET: book/Details/5
        public ActionResult Details(int id)
        {
            var bookToDetails = _db.books.Find(id);
            return View(bookToDetails);
        }

        // GET: book/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: book/Create
        [HttpPost]
        public ActionResult Create(books newBook)
        {
            string FileName = Path.GetFileNameWithoutExtension(newBook.ImageFile.FileName);
            string extension = Path.GetExtension(newBook.ImageFile.FileName);
            FileName = FileName + DateTime.Now.ToString("yymmssfff") + extension;
            newBook.url = "~/bookImage/" + FileName;
            FileName = Path.Combine(Server.MapPath("~/bookImage/"), FileName);
            newBook.ImageFile.SaveAs(FileName);
                
                _db.books.Add(newBook);
                _db.SaveChanges();
                ModelState.Clear();
                return View();
            
        }

        // GET: book/Edit/5
        public ActionResult Edit(int id)
        {
            var booksToEdit = _db.books.Find(id);
            return View(booksToEdit);
        }

        // POST: book/Edit/5
        [HttpPost]
        public ActionResult Edit(books booksToEdit)
        {
            var orginalBook = _db.books.Find(booksToEdit.id);
            try
            {

                if (TryUpdateModel(orginalBook, new string[] {
                    "author", "title", "publisher","quantity","Price","type","url" }))
                {
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(orginalBook);
            }
        }

        // GET: book/Delete/5
        public ActionResult Delete(int id)
        {
            var booksToDelete = _db.books.Find(id);
            return View(booksToDelete);
        }

        // POST: book/Delete/5
        [HttpPost]
        public ActionResult Delete(books bookToDelete)
        {
            try
            {
                // TODO: Add delete logic here
                var selBook = _db.books.Find(bookToDelete.id);
                if (!ModelState.IsValid)
                    return View(selBook);
                _db.books.Remove(selBook);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(bookToDelete);
            }
        }
    }
}