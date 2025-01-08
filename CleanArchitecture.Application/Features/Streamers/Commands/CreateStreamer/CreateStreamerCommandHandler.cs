using AutoMapper;
using CleanArchitecture.Application.Contracts.Infraestructure;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Streamers.Commands.CreateStreamer
{
    public class CreateStreamerCommandHandler : IRequestHandler<CreateStreamerCommand, int>
    {
        //private readonly IStreamerRepository _streamerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreateStreamerCommandHandler> _logger;

        public CreateStreamerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, ILogger<CreateStreamerCommandHandler> logger)
        {
            //_streamerRepository = streamerRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<int> Handle(CreateStreamerCommand request, CancellationToken cancellationToken)
        {
            var StreamerEntity = _mapper.Map<Streamer>(request);
            //var NewStreamer = await _streamerRepository.AddAsync(StreamerEntity);
            _unitOfWork.StreamerRepository.AddEntity(StreamerEntity);
            var result = await _unitOfWork.Complete();

            if(result <= 0)
            {
                throw new Exception("Failed to create the Streamer");
            }


            _logger.LogInformation($"Streamer {StreamerEntity.Name} was created.");
            await SendEmail(StreamerEntity);
            return StreamerEntity.Id;
        }

        private async Task SendEmail(Streamer streamer)
        {
            var email = new Email()
            {
                To = "vaxi.drez.social@gmail.com",
                Body = $"Streamer was created: {streamer.Name} with Id {streamer.Id}",
                Subject = "A new Streamer was created"
            };

            try
            {
                await _emailService.SendEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ocurred while sending email for Streamer {streamer.Name}. Error: {ex.Message}");
            }
        }
    }
}
