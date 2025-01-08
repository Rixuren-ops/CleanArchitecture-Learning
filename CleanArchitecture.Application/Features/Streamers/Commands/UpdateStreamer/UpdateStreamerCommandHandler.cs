using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.UpdateStreamer
{
    public class UpdateStreamerCommandHandler : IRequestHandler<UpdateStreamerCommand>
    {
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateStreamerCommandHandler> _logger;

        public UpdateStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateStreamerCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateStreamerCommand request, CancellationToken cancellationToken)
        {
            //var StreamerToUpdate = await _streamerRepository.GetByIdAsync(request.Id);
            var StreamerToUpdate = await _unitOfWork.StreamerRepository.GetByIdAsync(request.Id);
            if (StreamerToUpdate == null)
            {
                _logger.LogError("Streamer does not exist in the database.");
                throw new NotFoundException(nameof(Streamer), request.Id);
            }

            _mapper.Map(request, StreamerToUpdate, typeof(UpdateStreamerCommand), typeof(Streamer));

            //await _streamerRepository.UpdateAsync(StreamerToUpdate);
            _unitOfWork.StreamerRepository.UpdateEntity(StreamerToUpdate);
            var result = _unitOfWork.Complete();

            _logger.LogInformation($"Streamer {StreamerToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }
    }
}
