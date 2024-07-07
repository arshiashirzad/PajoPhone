using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PajoPhone.Migrations
{
    public partial class AddedDeletedAtToFieldKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDisabled",
                table: "FieldsKeys");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FieldsKeys",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FieldsKeys");

            migrationBuilder.AddColumn<bool>(
                name: "isDisabled",
                table: "FieldsKeys",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
