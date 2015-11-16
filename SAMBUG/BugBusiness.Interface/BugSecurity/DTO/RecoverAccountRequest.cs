namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class RecoverAccountRequest
    {
        public string EmailTo { get; set; }
        public string LinkPassword { get; set; }
    }
}
