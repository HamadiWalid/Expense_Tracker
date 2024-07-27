using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class CategoryController : Controller
    {
        

        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
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
            public IActionResult Create(Category obj)
            {
           
                    _context.Categories.Add(obj);
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

                var CategoryFromDb = _context.Categories.Find(id);
           
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

    
                    _context.Categories.Update(obj);
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

                var CategoryFromDb = _context.Categories.Find(id);
     
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
                var CategoryFromDb = _context.Categories.Find(id);

                if (CategoryFromDb == null)
                {
                    return NotFound();
                }


                _context.Categories.Remove(CategoryFromDb);
                _context.SaveChanges();


                return RedirectToAction("Index");

            }

        public IActionResult Index(string searchString)
        {
            var Categories = from c in _context.Categories select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                Categories = Categories.Where(c => c.Name.Contains(searchString));
            }

            ViewData["SearchString"] = searchString;
            return View(Categories.ToList());
        }

    }
}
    
