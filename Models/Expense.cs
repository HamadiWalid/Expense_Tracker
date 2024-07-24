using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.Models
{

    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
