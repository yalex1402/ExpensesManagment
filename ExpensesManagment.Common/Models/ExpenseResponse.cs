using System;
using System.Collections.Generic;
using System.Text;

namespace ExpensesManagment.Common.Models
{
    public class ExpenseResponse
    {
        public int Id { get; set; }

        public string Details { get; set; }

        public float Value { get; set; }

        public DateTime Date { get; set; }

        public DateTime DateToLocal => Date.ToLocalTime();

        public string PicturePath { get; set; }

        public ExpenseTypeResponse Type { get; set; }
    }
}
