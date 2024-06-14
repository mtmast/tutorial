namespace EmailService.Models
{
    public class MailData
    {
        public string? EmailToId { get; set; }
        public string? EmailToName { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }

        public MailData() { }
        public MailData(string emailToId, string emailToName, string emailBody, string emailSubject)
        {
            EmailToId = emailToId;
            EmailToName = emailToName;
            EmailBody = emailBody;
            EmailSubject = emailSubject;
        }

    }
}
