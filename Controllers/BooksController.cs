using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
  [Route("[controller]")]
  [ApiController]
  [Authorize]
  public class BooksController : ControllerBase
  {
    
    private readonly IBookRepository _repository;
    private readonly IMapper _mapper;

    public BooksController(IBookRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }
    

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
    {
      var books = await _repository.GetAll();
      return Ok(books);
    }

    [HttpGet("{id}", Name = "GetBookById")]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
      var bookResult = await _repository.GetBookById(id);
      if (bookResult == null)
      {
        return NotFound();
      }

      return Ok(bookResult);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<Book>> CreateBook(BookWriteDto book)
    {
      var newBook = _mapper.Map<Book>(book);
      newBook.IsAvailable = true;

      _repository.CreateBook(newBook);
      var isSuccess = await _repository.SaveChanges();

      if(!isSuccess)
      {
        return BadRequest();
      }

      return CreatedAtRoute(nameof(GetBookById), new { Id = newBook.Id }, newBook);

    }

    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateBook(int id, BookWriteDto updateBook)
    {
      var bookResult = await _repository.GetBookById(id);
      if (bookResult == null)
      {
        return NotFound();
      }

      //bookResult.Price = 5000;
      _mapper.Map(updateBook, bookResult);

      _repository.UpdateBook(bookResult);

      await _repository.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteBook(int id)
    {
      var bookResult = await _repository.GetBookById(id);
      if (bookResult == null)
      {
        return NotFound();
      }
      _repository.DeleteBook(bookResult);
      await _repository.SaveChanges();

      return NoContent();
    }
  }
}