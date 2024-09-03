namespace Initial_API.Models
{
    public class LoginConfirmationModel
    {
        public byte[] PasswordHash { set; get; }
        public byte[] PasswordSalt { set; get; }
    }
}
