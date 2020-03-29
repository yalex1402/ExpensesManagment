using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesManagment.Common.Helpers
{
    public interface IRegexHelper
    {
        bool IsValidEmail(string email);

        bool IsValidDate(string date);
    }
}
