namespace Initial_API.Models
{
    public class RegistrationModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirm { get; set; }
    }
}
