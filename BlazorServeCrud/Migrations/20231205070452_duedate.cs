using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorServeCrud.Migrations
{
    /// <inheritdoc />
    public partial class duedate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Task",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskComment_TaskId",
                table: "TaskComment",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComment_UserId",
                table: "TaskComment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComment_Task_TaskId",
                table: "TaskComment",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComment_User_UserId",
                table: "TaskComment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComment_Task_TaskId",
                table: "TaskComment");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComment_User_UserId",
                table: "TaskComment");

            migrationBuilder.DropIndex(
                name: "IX_TaskComment_TaskId",
                table: "TaskComment");

            migrationBuilder.DropIndex(
                name: "IX_TaskComment_UserId",
                table: "TaskComment");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Task");
        }
    }
}
