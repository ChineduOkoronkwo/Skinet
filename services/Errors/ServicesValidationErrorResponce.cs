using System.Collections.Generic;

namespace services.Errors
{
    public class ServicesValidationErrorResponce : ServiceResponse
    {
        public ServicesValidationErrorResponce() : base(400)
        {
        }

        public IEnumerable<string> Errors { get; set; }
    }
}