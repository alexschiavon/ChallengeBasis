﻿using Arch.Domain.Adapters.Helper;
using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Helper.Exceptions;
using BookDomain.Models;
using BookDomain.Repositories;
using BookDomain.Services;
using Microsoft.Extensions.Logging;

namespace BookApplication.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository repository;
        private readonly ILogger logger;
        private readonly IBookRepository bookRepository;

        public SubjectService(ISubjectRepository repository, ILoggerFactory loggerFactory, IBookRepository bookRepository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            logger = loggerFactory?.CreateLogger<SubjectService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            this.bookRepository = bookRepository;
        }

        public async Task<bool> Delete(Subject o)
        {
            //Implementação das regras de negócio se tiver um autor associado a um livro, não pode ser deletado
            Metadata<Book, BookFilter> metadataBooks = await bookRepository.FindByFilterAsync(new Metadata<Book, BookFilter> { Custom = new BookFilter { SubjectId = o.SubjectId.ToString() } });
            if (metadataBooks?.Pagination?.TotalCount > 0)
            {
                throw new ValidationException("Não é possível deletar um assunto que possui livros associados.");
            }

            var del = await repository.FindById(o.SubjectId);
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

        public Task<Metadata<Subject, BasicFilter>> FindByFilter(Metadata<Subject, BasicFilter> filter)
        {
            return repository.FindByFilterAsync(filter);
        }

        public async Task<Subject> FindById(Guid id)
        {
            return await repository.FindById(id);
        }

        public IBaseRepository<Subject, Guid, BasicFilter> Instance()
        {
            return this.repository;
        }

        public Task<Subject> SaveOrUpdate(Subject o)
        {
            this.Validate(o);
            return repository.SaveOrUpdate(o);
        }

        public void Validate(Subject o)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(o.Description) || o.Description.Length < 3 || o.Description.Length > 20)
            {
                errors.Add("Descrição precisa ter de 3 a 20 letras.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(string.Join("\n", errors));
            }
        }
    }
}
