using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

 

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
        
            _context.Users.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
    
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var UserFromDb = _context.Users.Find(id);
     

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

            _context.Users.Update(obj);
            _context.SaveChanges();

            return RedirectToAction("Index");
            
        }



        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var UserFromDb = _context.Users.Find(id);
    

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
            var UserFromDb = _context.Users.Find(id);

            if (UserFromDb == null)
            {
                return NotFound();
            }


            _context.Users.Remove(UserFromDb);
            _context.SaveChanges();


            return RedirectToAction("Index");

        }

        //GET: User/Index

        public IActionResult Index(string searchString)
        {
            var Users = from c in _context.Users select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                Users = Users.Where(c => c.Username.Contains(searchString));
            }

            ViewData["SearchString"] = searchString;
            return View(Users.ToList());
        }


    }
}
