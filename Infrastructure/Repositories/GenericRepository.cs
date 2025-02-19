using Castle.Core.Logging;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Specifications;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementation of IGenericRepository<T>
    /// Perform database operations read, add, update, delete.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        // ===================================
        // === Fields & Props
        // ===================================

        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<GenericRepository<T>> _logger;

        // ===================================
        // === Constructors
        // ===================================

        /// <summary>
        /// Injects a DbContext instance.
        /// </summary>
        /// <param name="dbContext"></param>
        public GenericRepository(ApplicationDbContext dbContext, ILogger<GenericRepository<T>> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        // ===================================
        // === Methods
        // ===================================

        /// <summary>
        /// Gets all items in the table asynchronously of type T.
        /// </summary>
        /// <returns>The task result contains a read only list of items of type T.
        /// </returns>
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Gets individual item based on its ID asynchronously of type T.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The task result contains an item with the specified ID</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// Gets an item with a specification asynchronously of type T.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>The task result contains an item based on a specification</returns>
        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets all item with a specification asynchronously of type T.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>The task result contains a list of items based on a specification</returns>
        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        /// <summary>
        /// Counts the number of items with a specification asynchronously of type T.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>The task result contains an integer as the count of the items</returns>
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        /// <summary>
        /// Applies specification to an entity.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns>A queryable set of the entity</returns>
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec).AsSplitQuery();
        }

        /// <summary>
        /// Adds new item of type T to the database.
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        /// <summary>
        /// Deletes an item of type T from the database.
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            _dbContext?.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Tracks the changes of an item in the database, only save when SaveChanges method was called.
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Save changes of the entries that were affected.
        /// </summary>
        /// <returns>A boolean true if one or more rows affected</returns>
        public async Task<bool> SaveAllAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
