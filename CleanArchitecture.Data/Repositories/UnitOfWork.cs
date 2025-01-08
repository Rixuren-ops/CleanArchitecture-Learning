using System.Collections;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Infraestructure.Persistence;

namespace CleanArchitecture.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable repositories;
        private readonly StreamerDbContext dbContext;

        private IVideoRepository videoRepository;
        private IStreamerRepository streamerRepository;

        public IVideoRepository VideoRepository => videoRepository ?? new VideoRepository(dbContext);
        public IStreamerRepository StreamerRepository => streamerRepository ?? new StreamerRepository(dbContext);

        public StreamerDbContext StreamerDbContext => dbContext;

        public UnitOfWork(StreamerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Complete()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public IAsyncRepository<TEntity> Repository<TEntity>() where TEntity : BaseDomainModel
        {
            if(repositories == null)
                repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryBase<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), dbContext);
                repositories[type] = repositoryInstance;
            }

            return (IAsyncRepository<TEntity>)repositories[type];
        }
    }
}
