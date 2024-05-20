using System;

namespace EmailLoader.Storage.Emails
{
    public class EmailProviderException : Exception
    {
        public EmailProviderException(string message) : base(message)
        {

        }
    }
}
