using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PajoPhone.Migrations
{
    public partial class isDisabledAddedToFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "FieldsKeys",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "isDisabled",
                table: "FieldsKeys",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_FieldsKeys_CategoryId_Key",
                table: "FieldsKeys",
                columns: new[] { "CategoryId", "Key" },
                unique: true,
                filter: "[isDisabled] = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FieldsKeys_CategoryId_Key",
                table: "FieldsKeys");

            migrationBuilder.DropColumn(
                name: "isDisabled",
                table: "FieldsKeys");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "FieldsKeys",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
