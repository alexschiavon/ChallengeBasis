using Arch.Domain.Adapters.Helper;
using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Models.Report;
using BookDomain.Repositories;
using BookDomain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public Task<ICollection<BookDetails>> Report()
        {
            return repository.Report();
        }

        public Task<Book> SaveOrUpdate(Book o)
        {
            this.Validate(o);
            return repository.SaveOrUpdate(o);
        }

        public void Validate(Book book)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(book.Title) || book.Title.Length < 3 || book.Title.Length > 40)
            {
                errors.Add("Titulo precisa ter de 3 a 40 letras.");
            }
            if (string.IsNullOrEmpty(book.Publisher) || book.Publisher.Length < 3 || book.Publisher.Length > 40)
            {
                errors.Add("Editora precisa ter de 3 a 40 letras.");
            }
            if (book.Edition <= 0)
            {
                errors.Add("Edição precisa ser maior que 0.");
            }
            if (book.PublicationYear == null || book.PublicationYear.Length != 4)
            {
                errors.Add("Ano de publicação precisa ter 4 dígitos.");
            }
            if (book.BookAuthors == null || book.BookAuthors.Count == 0)
            {
                errors.Add("Autor é obrigatório.");
            }
            if (book.BookSubjects == null || book.BookSubjects.Count == 0)
            {
                errors.Add("Assunto é obrigatório.");
            }
            // if (book.Price <= 0)
            // {
            //     errors.Add("Price is required and must be greater than 0.");
            // }
            // Add other validations as needed

            if (errors.Count > 0)
            {
                throw new ValidationException(string.Join("\n", errors));
            }

        }

    }
}
