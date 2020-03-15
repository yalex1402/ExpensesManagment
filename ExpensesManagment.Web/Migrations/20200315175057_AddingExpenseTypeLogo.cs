using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpensesManagment.Web.Migrations
{
    public partial class AddingExpenseTypeLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Expenses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Expenses");
        }
    }
}
