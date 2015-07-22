namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string EmailConfirmation { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string FarmName { get; set; }
    }
}
