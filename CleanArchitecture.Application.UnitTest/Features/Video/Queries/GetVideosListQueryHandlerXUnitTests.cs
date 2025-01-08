using AutoMapper;
using CleanArchitecture.Application.Features.Videos.Queries.GetVideosList;
using CleanArchitecture.Application.Mappings;
using CleanArchitecture.Application.UnitTests.Mocks;
using CleanArchitecture.Infraestructure.Repositories;
using Moq;
using Shouldly;
using Xunit;

namespace CleanArchitecture.Application.UnitTests.Features.Video.Queries
{
    public class GetVideosListQueryHandlerXUnitTests
    {
        private readonly IMapper mapper;
        private readonly Mock<UnitOfWork> mockUnitOfWork;

        public GetVideosListQueryHandlerXUnitTests()
        {
            this.mockUnitOfWork = MockUnitOfWork.GetUnitOfWork();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            this.mapper = mapperConfig.CreateMapper();

            MockVideoRepository.AddDataVideoRepository(this.mockUnitOfWork.Object.StreamerDbContext);
        }

        [Fact]
        public async Task GetVideosListTest()
        {
            var handler = new GetVideosListQueryHandler(this.mockUnitOfWork.Object, this.mapper);
            var result = await handler.Handle(new GetVideosListQuery("vaxidrez"), CancellationToken.None);
            result.ShouldBeOfType<List<VideosVm>>();
            result.Count.ShouldBe(1);
            //Assert.NotNull(result);
            //Assert.IsType<List<VideoListVm>>(result);
            //Assert.Equal(2, result.Count);
        }
    }
}
