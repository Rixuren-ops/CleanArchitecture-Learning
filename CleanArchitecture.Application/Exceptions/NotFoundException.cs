namespace CleanArchitecture.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object Key) : base($"Entity \"{name}\" ({Key}) was not found."){}
    }
}
