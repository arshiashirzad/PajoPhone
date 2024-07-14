using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PajoPhone.Migrations
{
    public partial class AddedDeletedTimeForCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FieldsKeys_CategoryId_Key",
                table: "FieldsKeys");

            migrationBuilder.AlterColumn<string>(
                name: "StringValue",
                table: "FieldsValues",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "FieldsKeys",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Categories",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "FieldsValues",
                keyColumn: "StringValue",
                keyValue: null,
                column: "StringValue",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "StringValue",
                table: "FieldsValues",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "FieldsKeys",
                keyColumn: "Key",
                keyValue: null,
                column: "Key",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "FieldsKeys",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FieldsKeys_CategoryId_Key",
                table: "FieldsKeys",
                columns: new[] { "CategoryId", "Key" },
                unique: true,
                filter: "[isDisabled] = 0");
        }
    }
}
