using Arch.Domain.Adapters.Helper;
using AutoMapper;
using BookApi.Models;
using BookDomain.Filters;
using BookDomain.Models;
using System.Security.Principal;

namespace BookApi.Helper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BookModel, Book>();
            CreateMap<Book, BookModel>();

            CreateMap<Metadata<Book, BasicFilter>, Metadata<BookModel, BasicFilter>>();
            CreateMap<Metadata<BookModel, BasicFilter>, Metadata<Book, BasicFilter>>();
        }
    }
}
