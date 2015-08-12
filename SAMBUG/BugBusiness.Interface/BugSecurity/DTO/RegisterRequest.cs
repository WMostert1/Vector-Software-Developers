namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string UsernameConfirmation { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string FarmName { get; set; }
    }
}
