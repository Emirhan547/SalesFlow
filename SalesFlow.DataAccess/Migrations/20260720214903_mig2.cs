using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItem_Customers_CustomerId",
                table: "TaskItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskItem_Customers_CustomerId1",
                table: "TaskItem");

            migrationBuilder.DropIndex(
                name: "IX_TaskItem_CustomerId1",
                table: "TaskItem");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "TaskItem");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItem_Customers_CustomerId",
                table: "TaskItem",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskItem_Customers_CustomerId",
                table: "TaskItem");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "TaskItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskItem_CustomerId1",
                table: "TaskItem",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItem_Customers_CustomerId",
                table: "TaskItem",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskItem_Customers_CustomerId1",
                table: "TaskItem",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
