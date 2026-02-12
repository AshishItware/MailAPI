namespace MailAPI.VmModel
{
    public class EmailRequest
    {
        public List<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
