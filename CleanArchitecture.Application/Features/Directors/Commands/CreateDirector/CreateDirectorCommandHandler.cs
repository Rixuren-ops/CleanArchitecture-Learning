using AutoMapper;
using CleanArchitecture.Application.Contracts.Persistence;
using CleanArchitecture.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Features.Directors.Commands.CreateDirector
{
    public class CreateDirectorCommandHandler : IRequestHandler<CreateDirectorCommand, int>
    {
        private readonly ILogger<CreateDirectorCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDirectorCommandHandler(ILogger<CreateDirectorCommandHandler> logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CreateDirectorCommand request, CancellationToken cancellationToken)
        {
            var director = _mapper.Map<Director>(request);

            _unitOfWork.Repository<Director>().AddEntity(director);
            var result = await _unitOfWork.Complete();

            if(result <= 0)
            {
                _logger.LogError("Director creation failed");
                throw new Exception("Director creation failed");

            }
            
            _logger.LogInformation($"Director created successfully with id: {director.Id}");
            return director.Id;
        }
    }    
}
