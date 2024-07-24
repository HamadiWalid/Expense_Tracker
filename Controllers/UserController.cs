using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<User> objUserList = _db.Users.ToList();
        //    return View(objUserList);
        //}


        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User obj)
        {
            //if(obj.Nom==null)
            //ModelState.AddModelError("Nom", "Champ obligatoire");

            //if (ModelState.IsValid)
            //{
            _db.Users.Add(obj);
            _db.SaveChanges();
            //TempData["success"] = "User added successfully";
            return RedirectToAction("Index");
            //}

            //return View(obj);

        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var UserFromDb = _db.Users.Find(id);
            //var UserFromDbFirst=_db.Users.FirstOrDefault(u=>u.Id==id);
            //var UserFromDbSingle=_db.Users.SingleOrDefault(u=>u.Id==id);

            if (UserFromDb == null)
            {
                return NotFound();
            }
            return View(UserFromDb);


        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User obj)
        {

            //if (ModelState.IsValid)
            //{
            _db.Users.Update(obj);
            _db.SaveChanges();
            //TempData["success"] = "User edited successfully";

            return RedirectToAction("Index");
            //}
            //else
            //{
            //    return View(obj);
            //}
        }



        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var UserFromDb = _db.Users.Find(id);
            //var UserFromDbFirst = _db.Users.FirstOrDefault(u => u.Id == id);
            //var UserFromDbSingle = _db.Users.SingleOrDefault(u => u.Id == id);

            if (UserFromDb == null)
            {
                return NotFound();
            }
            return View(UserFromDb);


        }

        //Post
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var UserFromDb = _db.Users.Find(id);

            if (UserFromDb == null)
            {
                return NotFound();
            }


            _db.Users.Remove(UserFromDb);
            _db.SaveChanges();

            //TempData["success"] = "User deleted successfully";

            return RedirectToAction("Index");

        }

        //GET: User/Index

        public IActionResult Index(string searchString)
        {
            // Query to retrieve Users
            var Users = from c in _db.Users select c;

            // If search string is not null or empty, filter Users based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(c => c.Username.Contains(searchString));
            }

            // Pass the Users and the search string to the view
            ViewData["SearchString"] = searchString;
            return View(Users.ToList());
        }


    }
}
