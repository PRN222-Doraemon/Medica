using Core.Entities;
using Core.Interfaces.Repos;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        // ===================================
        // === Fields & Props
        // ===================================

        private readonly ApplicationDbContext _dbContext;
        private readonly ILoggerFactory _loggerFactory;
        private ConcurrentDictionary<string, object> _repositories;

        // ===================================
        // === Constructors
        // ===================================

        public UnitOfWork(ApplicationDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _loggerFactory = loggerFactory;
        }

        // ===================================
        // === Methods
        // ===================================
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// For example type = "Customer", then 1 GenericRepository of type Customer is created or acccesed if it's created
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            // Avoid create repository too early
            if (_repositories == null)
            {
                _repositories = new ConcurrentDictionary<string, object>();
            }

            var typeName = typeof(T).Name;

            var repoInstanceTypeT = _repositories.GetOrAdd(typeName, _ =>
            {
                var repoType = typeof(GenericRepository<T>);
                var repoLogger = _loggerFactory.CreateLogger<GenericRepository<T>>();

                // Using reflection to create an instanceof GenericRepository with type T
                // Passing db context for each repository
                var repoInstance = Activator.CreateInstance(
                    repoType,
                    _dbContext,
                    repoLogger);

                return repoInstance;
            });

            // Return the specific repository pattern for the 
            return (IGenericRepository<T>)repoInstanceTypeT;
        }
    }
}
