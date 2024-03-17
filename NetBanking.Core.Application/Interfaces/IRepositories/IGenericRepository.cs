using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.Interfaces.IRepositories
{
    public interface IGenericRepository<Entity> where Entity : class
    {
        Task<Entity> SaveAsync(Entity entity);
        Task UpdateAsync(Entity entity, int Id);
        Task DeleteAsync(Entity entity);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GeEntityByIDAsync(int ID);
        Task<List<Entity>> FindAllAsync(Expression<Func<Entity, bool>> filter);
        Task<bool> ExistsAsync(Expression<Func<Entity, bool>> filter);
        Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties);
    }
}
