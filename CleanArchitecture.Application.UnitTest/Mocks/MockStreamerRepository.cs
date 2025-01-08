using AutoFixture;
using CleanArchitecture.Domain;
using CleanArchitecture.Infraestructure.Persistence;

namespace CleanArchitecture.Application.UnitTests.Mocks
{
    public static class MockStreamerRepository
    {
        public static void AddStreamerRepository(StreamerDbContext streamerDbContextFake)
        {
            var Fixture = new Fixture();
            Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var streamers = Fixture.CreateMany<Streamer>().ToList();
            streamers.Add(Fixture.Build<Streamer>()
                                 .With(v => v.Id, 8000)
                                 .Without(v => v.Videos)
                                 .Create());

            streamerDbContextFake.Streamers!.AddRange(streamers);
            streamerDbContextFake.SaveChanges();
        }
    }
}
