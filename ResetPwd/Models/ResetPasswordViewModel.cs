namespace ResetPwd.Models
{
    public class ResetPasswordViewModel
    {
        public string? Token { get; set; }
        public DateTime Expiry {  get; set; }
        public string? ResetEmail { get; set; }
    }
}
