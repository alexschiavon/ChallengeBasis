using Arch.Domain.Adapters.Helper;
using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Repositories;
using BookDomain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository repository;
        private readonly ILogger logger;

        public BookService(IBookRepository repository, ILoggerFactory loggerFactory)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            logger = loggerFactory?.CreateLogger<BookService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<bool> Delete(Book o)
        {
            //TODO: Implementar regras de negócio se tiver um autor associado a um livro, não pode ser deletado
            var del = await repository.FindById(o.BookId);
            if (del == null)
            {
                return false;
            }
            await repository.Delete(o);
            return true;
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        public Task<Metadata<Book, BasicFilter>> FindByFilter(Metadata<Book, BasicFilter> filter)
        {
            return repository.FindByFilterAsync(filter);
        }

        public async Task<Book> FindById(Guid id)
        {
            return await repository.FindById(id);
        }

        public IBaseRepository<Book, Guid, BasicFilter> Instance()
        {
            return this.repository;
        }

        public Task<Book> SaveOrUpdate(Book o)
        {
            this.Validate(o);
            return repository.SaveOrUpdate(o);
        }

        public void Validate(Book o)
        {
            //TODO: Implementar regras de negócio, validação insersão ou atualização de livro
        }
    }
}
