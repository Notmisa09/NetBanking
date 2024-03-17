using Microsoft.EntityFrameworkCore;
using NetBanking.Core.Application.Interfaces.IRepositories;
using NetBanking.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity> : IGenericRepository<Entity> where Entity : class
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<Entity> _entities;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _entities = context.Set<Entity>();
        }
        public virtual async Task DeleteAsync(Entity entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<Entity, bool>> filter)
        {
            return await _entities.AnyAsync(filter);
        }

        public virtual async Task<List<Entity>> FindAllAsync(Expression<Func<Entity, bool>> filter)
        {
            return await _entities.Where(filter).ToListAsync();
        }

        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<List<Entity>> GetAllWithIncludeAsync(List<string> properties)
        {
            var query = _context.Set<Entity>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Entity> GeEntityByIDAsync(int Id)
        {
            return await _entities.FindAsync(Id);
        }

        public virtual async Task<Entity> SaveAsync(Entity entity)
        {
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(Entity entity, int Id)
        {
            var entry = await _context.Set<Entity>().FindAsync(Id);
            _context.Entry(entry).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
