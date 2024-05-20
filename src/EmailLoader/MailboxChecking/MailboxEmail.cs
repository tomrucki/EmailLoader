using System.Collections.Generic;

namespace EmailLoader.MailboxChecking
{
    public class MailboxEmail 
    {
        public string EmailId { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public List<MailboxEmailAttachment> Attachments { get; set; }
    }

    public class MailboxEmailAttachment
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
