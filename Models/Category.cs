using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
