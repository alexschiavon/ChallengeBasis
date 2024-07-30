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
        private readonly ILogger logger;

        public AuthorService(IAuthorRepository repository, ILoggerFactory loggerFactory)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            logger = loggerFactory?.CreateLogger<AuthorService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<bool> Delete(Author o)
        {
            //TODO: Implementar regras de negócio se tiver um autor associado a um livro, não pode ser deletado
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
