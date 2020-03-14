using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpensesManagment.Web.Migrations
{
    public partial class AddingTripTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Expenses",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CityVisited = table.Column<string>(maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_TripId",
                table: "Expenses",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Trips_TripId",
                table: "Expenses",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Trips_TripId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_TripId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Expenses");
        }
    }
}
