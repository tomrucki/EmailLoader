using Microsoft.Azure.Cosmos.Table;

namespace EmailLoader.MailboxChecking
{
    public class MailboxEntity : TableEntity
    {
        public MailboxEntity()
        {
        }

        public MailboxEntity(string mailboxCheckerId, string address, int organizationId)
        {
            PartitionKey = mailboxCheckerId;
            RowKey = address;
            OrganizationId = organizationId;
        }

        public string MailboxCheckerId => PartitionKey;
        public string Address => RowKey;
        public int OrganizationId { get; set; }
    }
}
