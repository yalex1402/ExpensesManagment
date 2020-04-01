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

        public string PictureFullPath => string.IsNullOrEmpty(PicturePath)
                    ? "https://expensesmanagmentweb.azurewebsites.net//images/noimage.png"
                    : $"https://expensesmanagmentweb.azurewebsites.net{PicturePath.Substring(1)}";

        public ExpenseTypeResponse Type { get; set; }
    }
}
