using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Contexts;
using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Repositories
{
  public interface IBookRepository
  {
    Task<bool> SaveChanges();

    Task<IEnumerable<Book>> GetAll();
    Task<Book> GetBookById(int id);
    void CreateBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(Book book);
  }

  public class BookRepository : IBookRepository
  {

    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Book>> GetAll()
    {
      return await _context.Books.ToListAsync();
    }

    public async Task<Book> GetBookById(int id)
    {
      return await _context.Books.FindAsync(id);
    }

    public void CreateBook(Book book)
    {
      if (book == null)
      {
        throw new ArgumentNullException(nameof(book));
      }

      _context.Books.Add(book);
    }

    public void UpdateBook(Book book)
    {
      //Do nothing
    }
    
    public void DeleteBook(Book book)
    {
      if (book == null)
      {
        throw new ArgumentNullException(nameof(book));
      }
      _context.Books.Remove(book);
    }

    public async Task<bool> SaveChanges()
    {
      var result = await _context.SaveChangesAsync();

      return (result > 0);
    }
  }
}