using AutoFixture;
using CleanArchitecture.Domain;
using CleanArchitecture.Infraestructure.Persistence;

namespace CleanArchitecture.Application.UnitTests.Mocks
{
    public static class MockVideoRepository
    {
        public static void AddDataVideoRepository(StreamerDbContext streamerDbContextFake)
        {
            var Fixture = new Fixture();
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var videos = Fixture.CreateMany<Video>().ToList();
            videos.Add(Fixture.Build<Video>().With(v => v.CreatedBy, "vaxidrez").Create());
           
            streamerDbContextFake.Videos!.AddRange(videos);
            streamerDbContextFake.SaveChanges();
        }
    }
}