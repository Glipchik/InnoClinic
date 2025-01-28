namespace Documents.Application.Exceptions
{
    public class RelatedObjectFoundException : BadRequestException
    {
        public RelatedObjectFoundException()
            : base("Related object found.")
        {
        }

        public RelatedObjectFoundException(string message)
            : base(message)
        {
        }

        public RelatedObjectFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
