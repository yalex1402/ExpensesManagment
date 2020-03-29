using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ExpensesManagment.Common.Helpers
{
    public class RegexHelper : IRegexHelper
    {
        public bool IsValidEmail(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool IsValidDate(string date)
        {
            try
            {
                DateTime.Parse(date);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
