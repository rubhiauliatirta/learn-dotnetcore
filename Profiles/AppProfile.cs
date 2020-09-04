using AutoMapper;
using LibraryAPI.Dtos;
using LibraryAPI.Models;

namespace LibraryAPI.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            //Source -> Target
            CreateMap<BookWriteDto, Book>();
            CreateMap<Book, TransactionBookDto>();
            CreateMap<TransactionWriteDto, Transaction>();
            CreateMap<Transaction, TransactionReadDto>();
            CreateMap<ApplicationUser, UserReadDto>();

        }
    } 
}