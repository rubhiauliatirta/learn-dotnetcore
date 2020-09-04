using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Dtos;
using LibraryAPI.Models;
using LibraryAPI.Repositories;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
  [Route("[controller]")]
  [ApiController]
  [Authorize]
  public class TransactionsController : ControllerBase
  {
    
    
    private readonly ITransactionRepository _repository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TransactionsController(ITransactionRepository repository, IMapper mapper, IUserService userService)
    {
      _repository = repository;
      _mapper = mapper;
      _userService = userService;
    }
    

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionReadDto>>> GetAll()
    {
      var role = User.Claims.First(c => c.Type == ClaimTypes.Role).Value;
      var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
      
      IEnumerable<Transaction> transactions;

      if(role == "Admin")
      {
        transactions = await _repository.GetAll();
      }
      else
      {
        transactions = await _repository.GetUserTransactions(userId);
      }
       var result = _mapper.Map<IEnumerable<TransactionReadDto>>(transactions);
      return Ok(result);
    }

    [HttpGet("{id}", Name = "GetTransactionById")]
    public async Task<ActionResult<TransactionReadDto>> GetTransactionById(int id)
    {
      var transactionResult = await _repository.GetTransactionById(id);
      if (transactionResult == null)
      {
        return NotFound();
      }

      var result = _mapper.Map<TransactionReadDto>(transactionResult);

      return Ok(result);
    }

    [HttpPost, Authorize(Roles = "User")]
    public async Task<ActionResult<Transaction>> CreateTransaction(TransactionWriteDto transaction)
    {
      var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
      var newTransaction = new Transaction
      {
        BookId = transaction.BookId,
        TotalDays = transaction.TotalDays,
        UserId = userId,
        IsDone = false,
      };
      //var newTransaction = _mapper.Map<Transaction>(transaction);
      try
      {
         await _repository.CreateTransaction(newTransaction);
      }
      catch (NullReferenceException) {
        return BadRequest("Book/User not found");
      }

      await _repository.SaveChanges();
      var result = _mapper.Map<TransactionReadDto>(newTransaction);
      return CreatedAtRoute(nameof(GetTransactionById), new { Id = result.Id }, result);

    }

    [HttpPatch("{id}/done"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DoneTransaction(int id)
    {
      var transaction = await _repository.GetTransactionById(id);
      if (transaction == null)
      {
        return NotFound();
      }
      
      transaction.IsDone = true;
      _repository.UpdateTransaction(transaction);
      await _repository.SaveChanges();

      return NoContent();
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCommand(int id)
    {
      var transaction = await _repository.GetTransactionById(id);
      if (transaction == null)
      {
        return NotFound();
      }
      _repository.DeleteTransaction(transaction);
      await _repository.SaveChanges();

      return NoContent();
    }
  }
}