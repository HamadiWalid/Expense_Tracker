﻿using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
