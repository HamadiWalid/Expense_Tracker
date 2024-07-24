using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Data;
using Expense_Tracker.Models;

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
            ViewBag.CategoryId = new SelectList(_context.Expenses, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username");
            return View();
        }

        // POST: Expense/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense obj)
        {
  
            _context.Expenses.Add(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Expense/Edit/5
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
            ViewData["CategoryId"] = new SelectList(_context.Expenses, "Id", "Name", expense.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Password", expense.UserId);
            return View(expense);
        }

        // POST: Expense/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Expense obj)
        {
            _context.Expenses.Update(obj);
            _context.SaveChanges();
            //TempData["success"] = "User edited successfully";

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

            //TempData["success"] = "User deleted successfully";

            return RedirectToAction("Index");

        }

        //// GET: Expense
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Expenses.Include(e => e.Category).Include(e => e.User);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        public IActionResult Index(string searchString)
        {
            // Query to retrieve Expenses
            var Expenses = from e in _context.Expenses
                           .Include(e => e.Category)
                           .Include(e => e.User)
                           select e;

            // If search string is not null or empty, filter Expenses based on the search string
            if (!string.IsNullOrEmpty(searchString))
            {
                Expenses = Expenses.Where(c => c.Name.Contains(searchString) || c.Category.Name.Contains(searchString)|| c.User.Username.Contains(searchString));
            }

            // Pass the Expenses and the search string to the view
            ViewData["SearchString"] = searchString;
            return View(Expenses.ToList());
        }
    }
}
