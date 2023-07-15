using System.Net;

namespace admission_task.Exceptions
{
    public class CustomException : Exception
    {
        public List<string>? ErrorMessages { get; }

        public HttpStatusCode StatusCode { get; }

        public CustomException(string message, List<string>? errors = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            ErrorMessages = errors;
            StatusCode = statusCode;
        }
    }
    public class NotFoundException : CustomException
    {
        public NotFoundException(string message)
            : base(message, null, HttpStatusCode.NotFound)
        {
        }
    }
    public class ApplicationException : CustomException
    {
        public ApplicationException(string message, List<string>? errors = default)
            : base(message, errors, HttpStatusCode.BadRequest)
        {
        }
    }
}
