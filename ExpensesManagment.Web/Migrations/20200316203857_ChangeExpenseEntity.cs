using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpensesManagment.Web.Migrations
{
    public partial class ChangeExpenseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Expenses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Expenses",
                nullable: true);
        }
    }
}
