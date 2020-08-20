using System;

namespace services.Errors
{
    public class ServiceResponse
    {
        public ServiceResponse(int statusCode, string message = null) 
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode();
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        private string GetDefaultMessageForStatusCode()
        {
            return StatusCode switch 
            {
                400 => "A bad request you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                505 => "Errors are the path to the dark side. "
                + "Errors lead to anger. Anger leads to hate. Hate leads to career change",
                _ => null
            };            

        }
    }
}