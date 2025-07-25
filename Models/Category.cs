
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(5)")]
        public string Icon { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Type { get; set; } = null!;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}