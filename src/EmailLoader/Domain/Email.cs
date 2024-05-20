using System.Collections.Generic;

namespace EmailLoader.Domain
{
    public class Email
    {
        public string EmailId { get; set; }
        public string Recipient { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public List<Attachment> Attachments { get; set; }

        public class Attachment 
        {
            public string Name { get; set; }
            public byte[] Data { get; set; }
        }
    }
}
