using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain;
using CleanArchitecture.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.Repositories
{
    public class VideoRepository : RepositoryBase<Video>, IVideoRepository
    {
        public VideoRepository(StreamerDbContext dbContext) : base(dbContext){ }

        public async Task<Video> GetVideoByName(string name)
        {
            return await _dbContext.Set<Video>().Where(v => v.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Video>> GetVideosByUserName(string userName)
        {
            return await _dbContext.Videos.Where(v => v.CreatedBy == userName).ToListAsync();
        }
    }
}
