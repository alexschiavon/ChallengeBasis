using Arch.DataAccessAdapter.Repositories.Common;
using BookDataAccessAdapter.Context;
using BookDomain.Filters;
using BookDomain.Models;
using BookDomain.Repositories;

namespace BookDataAccessAdapter.Repositories
{
    public class SubjectRepository(BookContext dbContext) : BaseRepository<Subject, Guid, BasicFilter>(dbContext), ISubjectRepository
    {
        public override IQueryable<Subject> ApplyFilters(IQueryable<Subject> query, BasicFilter filter)
        {
            // query para filtrar por algum campos específico da entidade aqui aqui
            return query;
        }

        protected override Guid GetEntityId(Subject entity)
        {
            return entity.SubjectId;
        }
    }
}
