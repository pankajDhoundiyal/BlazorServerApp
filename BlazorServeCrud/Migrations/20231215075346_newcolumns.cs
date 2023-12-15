using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorServeCrud.Migrations
{
    /// <inheritdoc />
    public partial class newcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpertCategoryId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpertId",
                table: "Task",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAgreeWithDueDate",
                table: "Task",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpertHelpRequired",
                table: "Task",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Task",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpertCategoryId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ExpertId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "IsAgreeWithDueDate",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "IsExpertHelpRequired",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Task");
        }
    }
}
