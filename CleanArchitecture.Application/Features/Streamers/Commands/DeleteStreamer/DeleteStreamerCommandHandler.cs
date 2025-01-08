using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.DeleteStreamer
{
    public class DeleteStreamerCommandHandler : IRequestHandler<DeleteStreamerCommand>
    {
        //private readonly IStreamerRepository streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<DeleteStreamerCommandHandler> logger;

        public DeleteStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteStreamerCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<Unit> Handle(DeleteStreamerCommand request, CancellationToken cancellationToken)
        {
            //var StreamerToDelete = await streamerRepository.GetByIdAsync(request.Id);
            var StreamerToDelete = await _unitOfWork.StreamerRepository.GetByIdAsync(request.Id);

            if (StreamerToDelete == null)
            {
                logger.LogError("Streamer not found in database.");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

             _unitOfWork.StreamerRepository.DeleteEntity(StreamerToDelete);
            await _unitOfWork.Complete();

            logger.LogInformation($"Streamer {StreamerToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
