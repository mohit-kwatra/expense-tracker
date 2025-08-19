
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        [ForeignKey("Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        [MaxLength(75, ErrorMessage = "Note cannot exceed 75 characters")]
        public string? Note { get; set; }
        
        public DateTime Date { get; set; } = DateTime.Now;
    }
}