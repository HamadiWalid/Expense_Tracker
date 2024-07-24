using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        

        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        //public IActionResult Index()
        //{
        //    IEnumerable<Category> objCategoryList = _db.Categories.ToList();
        //    return View(objCategoryList);
        //}


        //Get
        public IActionResult Create()
            {
                return View();
            }

            //Post
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(Category obj)
            {
                //if(obj.Nom==null)
                //ModelState.AddModelError("Nom", "Champ obligatoire");

                //if (ModelState.IsValid)
                //{
                    _db.Categories.Add(obj);
                    _db.SaveChanges();
                    //TempData["success"] = "Category added successfully";
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

                var CategoryFromDb = _db.Categories.Find(id);
                //var CategoryFromDbFirst=_db.Categories.FirstOrDefault(u=>u.Id==id);
                //var CategoryFromDbSingle=_db.Categories.SingleOrDefault(u=>u.Id==id);

                if (CategoryFromDb == null)
                {
                    return NotFound();
                }
                return View(CategoryFromDb);


            }

            //Post
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Category obj)
            {

                //if (ModelState.IsValid)
                //{
                    _db.Categories.Update(obj);
                    _db.SaveChanges();
                    //TempData["success"] = "Category edited successfully";

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

                var CategoryFromDb = _db.Categories.Find(id);
                //var CategoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
                //var CategoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

                if (CategoryFromDb == null)
                {
                    return NotFound();
                }
                return View(CategoryFromDb);


            }

            //Post
            [HttpPost, ActionName("DeletePost")]
            [ValidateAntiForgeryToken]
            public IActionResult DeletePost(int? id)
            {
                var CategoryFromDb = _db.Categories.Find(id);

                if (CategoryFromDb == null)
                {
                    return NotFound();
                }


                _db.Categories.Remove(CategoryFromDb);
                _db.SaveChanges();

                //TempData["success"] = "Category deleted successfully";

                return RedirectToAction("Index");

            }

        public IActionResult Index(string searchString)
        {
            // Query to retrieve Categories
            var Categories = from c in _db.Categories select c;

            // If search string is not null or empty, filter Categories based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                Categories = Categories.Where(c => c.Name.Contains(searchString));
            }

            // Pass the Categories and the search string to the view
            ViewData["SearchString"] = searchString;
            return View(Categories.ToList());
        }

    }
}
    
