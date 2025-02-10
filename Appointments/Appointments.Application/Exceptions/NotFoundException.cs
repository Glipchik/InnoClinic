namespace Appointments.Application.Exceptions
{
    public class NotFoundException : BadRequestException
    {
        public NotFoundException()
            : base("Object not found.")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
