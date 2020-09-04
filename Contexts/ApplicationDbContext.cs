using LibraryAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Contexts
{
	public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions options) : base(options){}
		
		// public DbSet<ApplicationUser> Use {get; set;}
    public DbSet<Book> Books {get; set;}
    public DbSet<Transaction> Transactions {get; set;}
	}
}