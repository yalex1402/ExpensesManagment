using ExpensesManagment.Common.Models;

namespace ExpensesManagment.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
