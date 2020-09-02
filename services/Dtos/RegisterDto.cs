using System.ComponentModel.DataAnnotations;

namespace services.Dtos
{
    public class RegisterDto
    {
        [Required]
        [RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Email address is invalid")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
        ErrorMessage = "Password must be at least 6 characters and contains at least 1 Upper case letter, 1 lower case letter, 1 number and 1 non alphanumeric character")]
        public string Password { get; set; }
        
        [Required]
        public string DisplayName { get; set; }
    }
}