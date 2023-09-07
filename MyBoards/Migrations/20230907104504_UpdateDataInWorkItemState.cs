using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBoards.Migrations
{
    public partial class UpdateDataInWorkItemState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkItemState",
                keyColumn: "Value",
                keyValue: "OnHold",
                column: "Value",
                value: "On Hold"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WorkItemState",
                keyColumn: "Value",
                keyValue: "On Hold",
                column: "Value",
                value: "OnHold"
                );
        }
    }
}
