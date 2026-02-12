using MailAPI.VmModel;

namespace MailAPI.Services.IService
{
    public interface IEmailService
    {
        Task SendBulkEmailsAsync(EmailRequest request);
    }
}
