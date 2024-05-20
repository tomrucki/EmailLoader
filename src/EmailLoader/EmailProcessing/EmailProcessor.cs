using EmailLoader.Domain;
using System;
using System.Threading.Tasks;

namespace EmailLoader.EmailProcessing
{
    public class EmailProcessor 
    {
        private readonly IEmailStore emailStore;
        private readonly IExternalApiClient externalApiClient;

        public EmailProcessor(IEmailStore emailStore, IExternalApiClient externalApiClient)
        {
            this.emailStore = emailStore;
            this.externalApiClient = externalApiClient;
        }

        public async Task Process(string emailId) 
        {
            var email = await emailStore.GetEmail(emailId, includeAttachments: false);

            EmailParserResult parserResult;
            try
            {
                parserResult = EmailParser.Parse(email);
            }
            catch (Exception ex)
            {
                throw new EmailProcessorException(ex, "Failed to extract code from e-mail");
            }

            await externalApiClient.AddCode(parserResult.Code, parserResult.OrganizationId);
        }
    }

    public class EmailProcessorException : Exception
    {
        public EmailProcessorException(Exception ex, string message) : base(message, ex)
        {
        }
    }
}
