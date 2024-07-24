using Expense_Tracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Expense_Tracker.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

     
    }
}
