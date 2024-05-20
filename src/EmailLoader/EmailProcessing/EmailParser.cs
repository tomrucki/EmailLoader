using EmailLoader.Domain;

namespace EmailLoader.EmailProcessing
{
    public static class EmailParser 
    {
        public static EmailParserResult Parse(Email email) 
        {
            // todo:

            return EmailParserResult.Empty;
        }
    }

    public class EmailParserResult
    {
        public string Code { get; set; }
        public int OrganizationId { get; set; }

        public static EmailParserResult Empty { get; } = new EmailParserResult();
    }
}
