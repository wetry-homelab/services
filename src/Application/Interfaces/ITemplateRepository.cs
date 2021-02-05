using Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITemplateRepository
    {
        Task<Template[]> ReadsAsync();
        Task<Template> ReadAsync(Expression<Func<Template, bool>> predicate);
        Task<int> InsertAsync(Template entity);
        Task<int> UpdateAsync(Template entity);
    }
}
