using Arch.Domain.Adapters.Helper;
using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Helper.Exceptions;
using BookDomain.Models;
using BookDomain.Repositories;
using BookDomain.Services;
using Microsoft.Extensions.Logging;

namespace BookApplication.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository repository;
        private readonly IBookRepository bookRepository;
        private readonly ILogger logger;

        public AuthorService(IAuthorRepository repository, ILoggerFactory loggerFactory, IBookRepository bookRepository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            logger = loggerFactory?.CreateLogger<AuthorService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            this.bookRepository = bookRepository;
        }

        public async Task<bool> Delete(Author o)
        {
            //Implementação das regras de negócio se tiver um autor associado a um livro, não pode ser deletado
            Metadata<Book, BookFilter> metadataBooks = await bookRepository.FindByFilterAsync(new Metadata<Book, BookFilter> { Custom = new BookFilter { AuthorId = o.AuthorId.ToString() } });
            if (metadataBooks?.Pagination?.TotalCount > 0)
            {
                throw new ValidationException("Não é possível deletar um autor que possui livros associados.");
            }

            var del = await repository.FindById(o.AuthorId);
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

        public Task<Metadata<Author, BasicFilter>> FindByFilter(Metadata<Author, BasicFilter> filter)
        {
            return repository.FindByFilterAsync(filter);
        }

        public async Task<Author> FindById(Guid id)
        {
            return await repository.FindById(id);
        }

        public IBaseRepository<Author, Guid, BasicFilter> Instance()
        {
            return this.repository;
        }

        public Task<Author> SaveOrUpdate(Author o)
        {
            this.Validate(o);
            return repository.SaveOrUpdate(o);
        }

        public void Validate(Author o)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(o.Name) || o.Name.Length < 3 || o.Name.Length > 40)
            {
                errors.Add("Nome precisa ter de 3 a 40 letras.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(string.Join("\n", errors));
            }
        }
    }
}
