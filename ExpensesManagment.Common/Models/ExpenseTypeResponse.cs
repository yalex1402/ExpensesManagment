
namespace ExpensesManagment.Common.Models
{
    public class ExpenseTypeResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LogoPath { get; set; }

        public string LogoFullPath => string.IsNullOrEmpty(LogoPath)
                    ? "https://expensesmanagmentweb.azurewebsites.net//images/noimage.png"
                    : $"https://expensesmanagmentweb.azurewebsites.net{LogoPath.Substring(1)}";
    }
}
