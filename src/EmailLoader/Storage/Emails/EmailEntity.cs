using EmailLoader.Storage.Common.Tables;
using System.Collections.Generic;

namespace EmailLoader.Storage.Emails
{
    public class EmailEntity : BaseTableEntity
    {
        public const string DefaultRowKey = "0";

        public EmailEntity()
        {
        }

        public EmailEntity(string emailId, string recipient, string subject, string text, List<EmailEntityAttachment> attachments)
        {
            PartitionKey = emailId;
            RowKey = DefaultRowKey;
            Recipient = recipient;
            Subject = subject;
            Text = text;
            Attachments = attachments;
        }

        public string EmailId => RowKey;
        public string Recipient { get; set; }

        public string Subject { get; set; }
        
        public string Text { get; set; }

        [TableEntityComplexProperty]
        public List<EmailEntityAttachment> Attachments { get; set; }
    }

    public class EmailEntityAttachment
    {
        public string Name { get; set; }
        public string StorageName { get; set; }
    }
}
