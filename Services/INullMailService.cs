namespace DutchTreat.Services
{
    public interface INullMailService
    {
        void SendMail(string to, string subject, string body);
    }
}