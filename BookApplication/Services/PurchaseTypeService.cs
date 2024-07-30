using Arch.Domain.Adapters.Helper;
using Arch.Domain.Contracts.Repository;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Repositories;
using BookDomain.Services;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace BookApplication.Services
{
    public class PurchaseTypeService : IPurchaseTypeService
    {
        private readonly IPurchaseTypeRepository repository;
        private readonly ILogger logger;

        public PurchaseTypeService(IPurchaseTypeRepository repository, ILoggerFactory loggerFactory)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            logger = loggerFactory?.CreateLogger<PurchaseTypeService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task<bool> Delete(PurchaseType o)
        {
            //TODO: Implementar regras de negócio se tiver um autor associado a um livro, não pode ser deletado
            var del = await repository.FindById(o.PurchaseTypeId);
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

        public Task<Metadata<PurchaseType, BasicFilter>> FindByFilter(Metadata<PurchaseType, BasicFilter> filter)
        {
            return repository.FindByFilterAsync(filter);
        }

        public async Task<PurchaseType> FindById(Guid id)
        {
            return await repository.FindById(id);
        }

        public IBaseRepository<PurchaseType, Guid, BasicFilter> Instance()
        {
            return this.repository;
        }

        public Task<PurchaseType> SaveOrUpdate(PurchaseType o)
        {
            this.Validate(o);
            return repository.SaveOrUpdate(o);
        }

        public void Validate(PurchaseType o)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(o.Name) || o.Name.Length < 3 || o.Name.Length > 40)
            {
                errors.Add("Titulo precisa ter de 3 a 40 letras.");
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(string.Join("\n", errors));
            }

        }
    }
}
