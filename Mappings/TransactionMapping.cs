
using ExpenseTracker.DTOs;
using ExpenseTracker.Models;

namespace ExpenseTracker.Mappings
{
    public static class TransactionMapping
    {
        public static IQueryable<TransactionDto> ToTransactionDto(this IQueryable<Transaction> query)
        {
            return query.Select(t => new TransactionDto
            {
                TransactionId = t.TransactionId,
                Category = new CategoryDto
                {
                    CategoryId = t.Category!.CategoryId,
                    Title = t.Category.Title,
                    Icon = t.Category.Icon,
                    Type = t.Category.Type
                },
                Amount = t.Amount,
                Note = t.Note,
                Date = t.Date
            });
        }

        public static TransactionDto GetTransactionDtoInstance(Transaction t)
        {
            return new TransactionDto
            {
                TransactionId = t.TransactionId,
                Category = new CategoryDto
                {
                    CategoryId = t.Category!.CategoryId,
                    Title = t.Category.Title,
                    Icon = t.Category.Icon,
                    Type = t.Category.Type
                },
                Amount = t.Amount,
                Note = t.Note,
                Date = t.Date
            };
        }
    }
}