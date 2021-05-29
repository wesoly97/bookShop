using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using bookShopProject.Models;
using bookShopProject.ViewModel;

namespace bookShopProject.Controllers
{
    public class BookController : Controller
    {
        private bookShopEntities _db = new bookShopEntities();
        private cart cartObject = new cart();
        // GET: book
        public ActionResult Index()
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("~/user/Login");
            }
            List<Category> categoryList = _db.Category.ToList();
            ViewBag.CategoryList = categoryList;
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem {  Text = "A-Z", Value = "AZ"});
            items.Add(new SelectListItem { Text = "Z-A", Value = "ZA" });
            items.Add(new SelectListItem { Text = "Cena rosnąco", Value = "cr" });
            items.Add(new SelectListItem { Text = "Cena Malejąco", Value = "cm" });

            ViewData["items"] = items;
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
            List<Category> categoryList = _db.Category.ToList();
            ViewBag.CategoryList = new SelectList(categoryList, "Id", "Type");
            
            return View();
        }

        // POST: book/Create
        [HttpPost]
        public ActionResult Create(books newBook)
        {
            List<Category> categoryList = _db.Category.ToList();
            ViewBag.CategoryList = new SelectList(categoryList, "Id", "Type");
            string FileName = Path.GetFileNameWithoutExtension(newBook.ImageFile.FileName);
            string extension = Path.GetExtension(newBook.ImageFile.FileName);
            FileName = FileName + DateTime.Now.ToString("yymmssfff") + extension;
            newBook.url = "~/bookImage/" + FileName;
            FileName = Path.Combine(Server.MapPath("~/bookImage/"), FileName);
            newBook.ImageFile.SaveAs(FileName);
                
                _db.books.Add(newBook);
                _db.SaveChanges();
                ModelState.Clear();
            return RedirectToAction("Index");

        }

        // GET: book/Edit/5
        public ActionResult Edit(int id)
        {
            List<Category> categoryList = _db.Category.ToList();
            ViewBag.CategoryList = new SelectList(categoryList, "Id", "Type");
            var booksToEdit = _db.books.Find(id);
            return View(booksToEdit);
        }

        // POST: book/Edit/5
        [HttpPost]
        public ActionResult Edit(books booksToEdit)
        {
            List<Category> categoryList = _db.Category.ToList();
            ViewBag.CategoryList = new SelectList(categoryList, "Id", "Type");
            var orginalBook = _db.books.Find(booksToEdit.id);
            try
            {

                if (TryUpdateModel(orginalBook, new string[] {
                    "author", "title", "publisher","quantity","Price","type","url","Category_id" }))
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
                var selBook = _db.books.Find(bookToDelete.id);
                string path = selBook.url;
                System.IO.File.Delete(Server.MapPath(path));
                _db.Order.RemoveRange(_db.Order.Where(c => c.books_id == bookToDelete.id));
                _db.SaveChanges();
                _db.cart.RemoveRange(_db.cart.Where(c => c.books_id == bookToDelete.id));
                _db.SaveChanges();
                _db.books.Remove(selBook);
                _db.SaveChanges();
                
            return RedirectToAction("Index");

        }
        public ActionResult showBookByCategory(string type)
        {
            return View(_db.books.Where(c => c.Category.Type == type && (c.quantity>0)));
        }
        public ActionResult cart()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            return View(_db.cart.ToList());
        }

        public JsonResult addToCart(int itemId, int userId)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
           
            int itemIdNumber = Convert.ToInt32(itemId);
            books bookObject = _db.books.Single(model=> model.id== itemIdNumber);
            var cartElement = _db.cart.Where(x => x.User_id == userId && x.books_id == itemId).FirstOrDefault();

            if (cartElement != null)
            {
                if (cartElement.quantity + 1 <= bookObject.quantity)
                {
                    cartElement.quantity = cartElement.quantity + 1;
                    cartElement.price = cartElement.price + bookObject.Price;
                }
                
                
            }
            else
            {
                cartObject.books_id = bookObject.id;
                cartObject.quantity = 1;
                cartObject.price = bookObject.Price;
                cartObject.User_id = Convert.ToInt32(userId);
                _db.cart.Add(cartObject);
            }
            _db.SaveChanges();
            cartElement = _db.cart.Where(x => x.User_id == userId && x.books_id == itemId).FirstOrDefault();

            return Json(new { success=true, quantity= cartElement.quantity, price = cartElement.price, priceBook = cartElement.books.Price,quantityBook= bookObject.quantity },JsonRequestBehavior.AllowGet);
        }
        public JsonResult removeFromCart(int itemId, int userId)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            
            int itemIdNumber = Convert.ToInt32(itemId);
            books bookObject = _db.books.Single(model => model.id == itemIdNumber);
            var cartElement = _db.cart.Where(x => x.User_id == userId && x.books_id == itemId).FirstOrDefault();
            int quantityTmp = 0;
            decimal priceTmp = 0;
            decimal bookPriceTmp=0;
            if (cartElement != null)
            {
                 quantityTmp = cartElement.quantity;
                 priceTmp = cartElement.price;
                 bookPriceTmp = cartElement.books.Price;
                if (cartElement.quantity > 0)
                {
                    cartElement.quantity = cartElement.quantity - 1;
                    cartElement.price = cartElement.price - bookObject.Price;
                    quantityTmp = cartElement.quantity;
                    priceTmp = cartElement.price;
                }
                if (cartElement.quantity == 0)
                {
                    _db.cart.RemoveRange(_db.cart.Where(c => c.id == cartElement.id));
                }
                _db.SaveChanges();
            }
            return Json(new { success = true, quantity = quantityTmp, price = priceTmp, priceBook = bookPriceTmp }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult cleanCart()
        {
            _db.cart.RemoveRange(_db.cart.ToList());
            _db.SaveChanges();
            return Json(new { success = true}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult showOrder()
        {
            ViewBag.length = _db.Order.ToList().Count;

            return View(_db.Order.ToList());
        }
        public ActionResult addOrder(int userid)
        {
            Order newOrder=new Order();
            var rand = new Random();
            var cartElement = _db.cart.Where(x => x.User_id == userid).ToList();
            int numberRand = rand.Next(1000);
            DateTime todaysDate = DateTime.Now;
            string stringDate = Convert.ToString(todaysDate);
            stringDate = Regex.Replace(stringDate, @"\s+", "");

            foreach (cart element in cartElement)
            {
                
                newOrder.books_id = element.books_id;
                newOrder.books_quantity = Convert.ToString(element.quantity);
                newOrder.User_id = userid;
                newOrder.status = "W trakcie realizacji";
                newOrder.totalAmount = Math.Round(element.books.Price * element.quantity,2);
                newOrder.order_number = stringDate + Convert.ToString(numberRand);
                var book = _db.books.Where(x => x.id ==element.books_id ).FirstOrDefault(); ;
                book.quantity = book.quantity - element.quantity;
                _db.Order.Add(newOrder);
                _db.SaveChanges();
            }
            
            _db.cart.RemoveRange(_db.cart.ToList());
            
            _db.SaveChanges();
            return RedirectToAction("showOrder");
        }
        public ActionResult showOneOrder(int userId, string orderNumber)
        {       
            
            return View(_db.Order.Where(c => c.User_id == userId && c.order_number == orderNumber));
        }

        [HttpPost]
        public ActionResult searchForBook(string keywords)
        {
            return View(_db.books.Where(c => c.title.Contains(keywords)|| c.author.Contains(keywords)));

        }
        public ActionResult changeSortingOption(string SortingValue)
        {
            List<books> book;
            List<SelectListItem> items = new List<SelectListItem>();
            if (SortingValue=="AZ")
            {
                items.Add(new SelectListItem { Text = "A-Z", Value = "AZ" });
                items.Add(new SelectListItem { Text = "Z-A", Value = "ZA" });
                items.Add(new SelectListItem { Text = "Cena rosnąco", Value = "cr" });
                items.Add(new SelectListItem { Text = "Cena Malejąco", Value = "cm" });
                book = _db.books.OrderBy(x => x.title).ToList();
            }
            else if (SortingValue=="ZA")
            {
                items.Add(new SelectListItem { Text = "Z-A", Value = "ZA" });
                items.Add(new SelectListItem { Text = "A-Z", Value = "AZ" });
                items.Add(new SelectListItem { Text = "Cena rosnąco", Value = "cr" });
                items.Add(new SelectListItem { Text = "Cena Malejąco", Value = "cm" });
                book = _db.books.OrderByDescending(x => x.title).ToList();
            }
            else if (SortingValue=="cm")
            {
                items.Add(new SelectListItem { Text = "Cena Malejąco", Value = "cm" });
                items.Add(new SelectListItem { Text = "A-Z", Value = "AZ" });
                items.Add(new SelectListItem { Text = "Cena rosnąco", Value = "cr" });
                items.Add(new SelectListItem { Text = "Z-A", Value = "ZA" });
                book = _db.books.OrderByDescending(x => x.Price).ToList();
            }
            else
            {
                items.Add(new SelectListItem { Text = "Cena rosnąco", Value = "cr" });
                items.Add(new SelectListItem { Text = "A-Z", Value = "AZ" });
                items.Add(new SelectListItem { Text = "Z-A", Value = "ZA" });
                items.Add(new SelectListItem { Text = "Cena Malejąco", Value = "cm" });
                book = _db.books.OrderBy(x => x.Price).ToList();
            }
         
            List<Category> categoryList = _db.Category.ToList();
            ViewBag.CategoryList = categoryList;
            ViewData["items"] = items;
            return PartialView("Index", book);
        }
    }
}