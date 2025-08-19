
using ExpenseTracker.Data;
using ExpenseTracker.Filters;
using ExpenseTracker.Mappings;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        // GET : api/transaction
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToTransactionDto()
                .ToListAsync();
            return Ok(transactions);
        }

        // POST : api/transaction
        [HttpPost]
        [ValidateCreateTransaction]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var createdTransaction = await _context.Transactions
                .Include(t => t.Category)
                .ToTransactionDto()
                .FirstOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);

            return Ok(createdTransaction);
        }

        // PUT : api/transaction/1
        [HttpPut("{id}")]
        [ValidateUpdateTransaction]
        [ValidateCreateTransaction]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] Transaction transaction)
        {
            var existingTransaction = await _context.Transactions.FindAsync(id);

            existingTransaction!.CategoryId = transaction.CategoryId;
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Note = transaction.Note;
            existingTransaction.Date = transaction.Date;

            await _context.SaveChangesAsync();

            var transactionDto = TransactionMapping.GetTransactionDtoInstance(await _context.Transactions.Include(t => t.Category).FirstAsync(t => t.TransactionId == id));
            return Ok(transactionDto);
        }

        // DELETE : api/transaction/1
        [HttpDelete("{id}")]
        [ValidateUpdateTransaction]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            _context.Transactions.Remove(transaction!);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }
    }
}