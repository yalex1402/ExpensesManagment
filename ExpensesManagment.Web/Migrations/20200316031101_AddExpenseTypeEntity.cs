using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpensesManagment.Web.Migrations
{
    public partial class AddExpenseTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseName",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseTypeId",
                table: "Expenses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    LogoPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ExpenseTypeId",
                table: "Expenses",
                column: "ExpenseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_ExpenseTypes_ExpenseTypeId",
                table: "Expenses",
                column: "ExpenseTypeId",
                principalTable: "ExpenseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_ExpenseTypes_ExpenseTypeId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "ExpenseTypes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ExpenseTypeId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ExpenseTypeId",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseName",
                table: "Expenses",
                nullable: false,
                defaultValue: 0);
        }
    }
}
