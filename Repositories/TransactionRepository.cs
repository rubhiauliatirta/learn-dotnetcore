using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Contexts;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;
using ServiceReference;
using static ServiceReference.CalculatorSoapClient;

namespace LibraryAPI.Repositories
{
  public interface ITransactionRepository
  {
    Task<bool> SaveChanges();

    Task<IEnumerable<Transaction>> GetAll();
    Task<IEnumerable<Transaction>> GetUserTransactions(string userId);
    Task<Transaction> GetTransactionById(int id);
    Task CreateTransaction(Transaction transaction);
    void UpdateTransaction(Transaction transaction);
    void DeleteTransaction(Transaction transaction);

  }

  public class TransactionRepository : ITransactionRepository
  {
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAll()
    {
      return await _context.Transactions
        .Include(t => t.Book)
        .Include(t => t.User)
        .ToListAsync();
    }
    
    public async Task<IEnumerable<Transaction>> GetUserTransactions(string userId)
    {
      return await _context.Transactions
        .Include(t => t.Book)
        .Include(t => t.User)
        .Where(t => t.UserId == userId)
        .ToListAsync();
    }

    public async Task<Transaction> GetTransactionById(int id)
    {
      return await _context.Transactions.FindAsync(id);
    }

    
    public async Task CreateTransaction(Transaction transaction)
    {
      if (transaction == null)
      {
        throw new ArgumentNullException(nameof(transaction));
      }
      var book =  await _context.Books.FindAsync(transaction.BookId);
      var user =  await _context.Users.FindAsync(transaction.UserId);

      if( book == null ){
        
      }
      var ec  = new EndpointConfiguration();
      // if(book == null){
      //   throw new ArgumentNullException("Book does not exist!");
      // }
      var calculatorClient = new CalculatorSoapClient(ec);
      transaction.TotalPrice = await calculatorClient.MultiplyAsync(book.Price, transaction.TotalDays);
      _context.Transactions.Add(transaction);
      book.IsAvailable = false;
      
    }

    public void DeleteTransaction(Transaction transaction)
    {
      if (transaction == null)
      {
        throw new ArgumentNullException(nameof(transaction));
      }
      _context.Transactions.Remove(transaction);
    }
    
    public void UpdateTransaction(Transaction transaction)
    {
      //do nothing
    }

    public async Task<bool> SaveChanges()
    {
      var result = await _context.SaveChangesAsync();

      return (result > 0);
    }
  }
}