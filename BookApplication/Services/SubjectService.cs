using Arch.Domain.Adapters.Helper;
using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
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

        public SubjectService(ISubjectRepository repository, ILoggerFactory loggerFactory)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            logger = loggerFactory?.CreateLogger<SubjectService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<bool> Delete(Subject o)
        {
            //TODO: Implementar regras de negócio se tiver um autor associado a um livro, não pode ser deletado
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
            //TODO: Implementar regras de negócio, validação insersão ou atualização
        }
    }
}
