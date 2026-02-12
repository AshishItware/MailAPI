using MailAPI.Services.IService;
using MailAPI.VmModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace MailAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendBulkEmailsAsync(EmailRequest request)
        {
            var settings = _config.GetSection("EmailSettings");

            using var client = new SmtpClient();

            await client.ConnectAsync(
                settings["SmtpServer"],
                int.Parse(settings["SmtpPort"]),
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                settings["SenderEmail"],
                settings["AppPassword"]);

            foreach (var recipient in request.Recipients)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    settings["SenderName"],
                    settings["SenderEmail"]));

                message.To.Add(MailboxAddress.Parse(recipient));
                message.Subject = request.Subject;

                var builder = new BodyBuilder
                {
                    TextBody = request.Body
                };

                //if (!string.IsNullOrEmpty(request.AttachmentPath))
                //    builder.Attachments.Add(request.AttachmentPath);
                if (request.Attachment != null && request.Attachment.Length > 0)
                {
                    using var stream = request.Attachment.OpenReadStream();

                    builder.Attachments.Add(
                        request.Attachment.FileName,
                        stream
                    );
                }

                message.Body = builder.ToMessageBody();

                await client.SendAsync(message);
                await Task.Delay(500);
            }

            await client.DisconnectAsync(true);
        }
    }
}
