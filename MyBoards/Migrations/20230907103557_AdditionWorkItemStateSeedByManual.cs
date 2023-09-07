using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards.Migrations
{
    public partial class AdditionWorkItemStateSeedByManual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkItemState",
                column: "Value",
                value: "OnHold"
                );

            migrationBuilder.InsertData(
                table: "WorkItemState",
                column: "Value",
                value: "Rejected"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkItemState",
                keyColumn: "Value",
                keyValue: "OnHold"
                );

            migrationBuilder.DeleteData(
                table: "WorkItemState",
                keyColumn: "Value",
                keyValue: "Rejected"
                );

        }
    }
}
