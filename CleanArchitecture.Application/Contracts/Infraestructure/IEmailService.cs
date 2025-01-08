using CleanArchitecture.Application.Models;

namespace CleanArchitecture.Application.Contracts.Infraestructure
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(Email email);
    }
}
