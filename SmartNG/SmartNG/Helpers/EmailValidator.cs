using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace SmartNG.Helpers
{
    public class EmailValidator
    {
        private string _Email { get; set; } = string.Empty;

        public EmailValidator(string email)
        {
            _Email = email;
        }

        public bool IsValid()
        {
            try
            {
                MailAddress m = new MailAddress(_Email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
