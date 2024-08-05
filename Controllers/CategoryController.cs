using Expense_Tracker.Data;
using Expense_Tracker.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;

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

        // GET: categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string search, DateTime? dateStart, DateTime? dateEnd, int pg = 1)
        {
            ViewData["CategorySortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
       

            var categories = from x in _context.Categories
                         select x;

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recCount = categories.Count();
            var pages = new Pages(recCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            categories = categories.Skip(recSkip).Take(pages.PageSize);
            this.ViewBag.Pages = pages;


            switch (sortOrder)
            {
                case "Name":
                    categories = categories.OrderBy(s => s.Name);
                    break;
                case "Name_desc":
                    categories = categories.OrderByDescending(s => s.Name);
                    break;

              
            }



            ViewData["SearchString"] = search;
        

            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(x => x.Name.Contains(search));
            }

        


            return View(await categories.AsNoTracking().ToListAsync());

        }


        // Action to export data to Excel
        public IActionResult ExportToExcel()
        {
            var categories = _context.Categories.ToList();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Categories");
                worksheet.Cells.LoadFromCollection(categories, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Categories-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
     
    }
}

    
