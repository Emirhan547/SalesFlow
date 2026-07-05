using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesFlow.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_AspNetUsers_CreatedById",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_CreatedById",
                table: "Meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTags",
                table: "CustomerTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CustomerTags");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CustomerTags");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CustomerTags");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "CustomerTags");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Meetings",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedUserId",
                table: "WorkItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Notes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "Meetings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Meetings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AssignedUserId",
                table: "Leads",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedUserId",
                table: "Deals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTags",
                table: "CustomerTags",
                columns: new[] { "CustomerId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_AssignedUserId",
                table: "Meetings",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_AspNetUsers_AssignedUserId",
                table: "Meetings",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetings_AspNetUsers_AssignedUserId",
                table: "Meetings");

            migrationBuilder.DropIndex(
                name: "IX_Meetings_AssignedUserId",
                table: "Meetings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerTags",
                table: "CustomerTags");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Meetings");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Meetings",
                newName: "CreatedById");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedUserId",
                table: "WorkItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignedUserId",
                table: "Leads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AssignedUserId",
                table: "Deals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CustomerTags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CustomerTags",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CustomerTags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "CustomerTags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerTags",
                table: "CustomerTags",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_CreatedById",
                table: "Meetings",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetings_AspNetUsers_CreatedById",
                table: "Meetings",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
