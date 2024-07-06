using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PajoPhone.Migrations
{
    public partial class AddedDeletedTimeForFieldValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FieldsValues",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FieldsValues");
        }
    }
}
