using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

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

        //GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string search, DateTime? dateStart, DateTime? dateEnd, int pg = 1)
        {
            ViewData["UsernameSortParm"] = sortOrder == "Username" ? "Username_desc" : "Username";
     

            var Users = from x in _context.Users
                             select x;

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recCount = Users.Count();
            var pages = new Pages(recCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            Users = Users.Skip(recSkip).Take(pages.PageSize);
            this.ViewBag.Pages = pages;


            switch (sortOrder)
            {
                case "Username":
                    Users = Users.OrderBy(s => s.Username);
                    break;
                case "Username_desc":
                    Users = Users.OrderByDescending(s => s.Username);
                    break;


            }



            ViewData["SearchString"] = search;


            if (!string.IsNullOrEmpty(search))
            {
                Users = Users.Where(x => x.Username.Contains(search));
            }




            return View(await Users.AsNoTracking().ToListAsync());

        }

        // Action to export data to Excel
        public IActionResult ExportToExcel()
        {
            var users = _context.Users.ToList();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Users");
                worksheet.Cells.LoadFromCollection(users, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Users-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
