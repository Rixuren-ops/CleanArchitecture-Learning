using CleanArchitecture.Domain;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infraestructure.Persistence
{
    public class StreamerDbContextSeed
    {
        public static async Task SeedAsync(StreamerDbContext streamerDbContext, ILogger<StreamerDbContextSeed> logger)
        {
            if (!streamerDbContext.Streamers!.Any())
            {
                streamerDbContext.Streamers!.AddRange(GetPreconfiguredStreamers());
                await streamerDbContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(StreamerDbContext).Name);
            }
        }
        private static IEnumerable<Streamer> GetPreconfiguredStreamers()
        {
            return new List<Streamer>
            {
                new Streamer() { CreatedBy =  "vaxidrez", Name = "Maxi HBP", Url = "http://ww.hbd.com", },
                new Streamer() { CreatedBy =  "vaxidrez", Name = "AmazonVip", Url = "http://amazonvip.com",},
            };
        }
    }
}
