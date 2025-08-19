
namespace ExpenseTracker.DTOs
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}