namespace Appointments.Application.Exceptions
{
    public class RelatedObjectNotFoundException : BadRequestException
    {
        public RelatedObjectNotFoundException()
            : base("Related object not found.")
        {
        }

        public RelatedObjectNotFoundException(string message)
            : base(message)
        {
        }

        public RelatedObjectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
