namespace services.Errors
{
    public class ServiceException : ServiceResponse
    {
        public ServiceException(int statusCode, string message = null, string detials = null) 
            : base(statusCode, message)
        {
            Details = detials;
        }
        public string Details { get; set; }
    }
} 