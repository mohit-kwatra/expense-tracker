
namespace ExpenseTracker.DTOs
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public CategoryDto Category { get; set; } = new CategoryDto();
        public int Amount { get; set; }
        public string? Note { get; set; }
        public DateTime Date { get; set; }
    }
}