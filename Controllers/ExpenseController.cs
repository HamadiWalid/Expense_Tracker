using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using OfficeOpenXml;

namespace Expense_Tracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }




        // GET: Expense/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username");
            return View();
        }

        // POST: Expense/Create
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense obj)
        {

            _context.Expenses.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Expense/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", expense.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", expense.UserId);
            return View(expense);
        }


        // POST: Expense/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Expense obj)
        {
            _context.Expenses.Update(obj);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Expense/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        //Post
        [HttpPost, ActionName("DeletePost")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var UserFromDb = _context.Expenses.Find(id);

            if (UserFromDb == null)
            {
                return NotFound();
            }


            _context.Expenses.Remove(UserFromDb);
            _context.SaveChanges();


            return RedirectToAction("Index");

        }

     
        //GET: Expense
        public async Task<IActionResult> Index()
        {
            return View(await _context.Expenses.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string search, DateTime? dateStart, DateTime? dateEnd, int pg = 1)
        {
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";


            // Query to retrieve Expenses
            var Expenses = from e in _context.Expenses
                           .Include(e => e.Category)
                           .Include(e => e.User)
                           select e;

            const int pageSize = 5;
            if (pg < 1)
                pg = 1;

            int recCount = Expenses.Count();
            var pages = new Pages(recCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            Expenses = Expenses.Skip(recSkip).Take(pages.PageSize);
            this.ViewBag.Pages = pages;


            switch (sortOrder)
            {
                case "Name":
                    Expenses = Expenses.OrderBy(s => s.Name);
                    break;
                case "Name_desc":
                    Expenses = Expenses.OrderByDescending(s => s.Name);
                    break;


            }



            ViewData["SearchString"] = search;


            if (!string.IsNullOrEmpty(search))
            {
                Expenses = Expenses.Where(x => x.Name.Contains(search));
            }




            return View(await Expenses.AsNoTracking().ToListAsync());

        }


        // Action to export data to Excel
        public IActionResult ExportToExcel()
        {
            var expenses = _context.Expenses.ToList();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Expenses");
                worksheet.Cells.LoadFromCollection(expenses, true);
                package.Save();
            }

            stream.Position = 0;
            string excelName = $"Expenses-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}