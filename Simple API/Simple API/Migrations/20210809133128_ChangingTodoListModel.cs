using Microsoft.EntityFrameworkCore.Migrations;

namespace Simple_API.Migrations
{
    public partial class ChangingTodoListModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_UserInfo_UserInfoUserId",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_UserInfoUserId",
                table: "ToDoList");

            migrationBuilder.Sql("ALTER TABLE ToDoList RENAME COLUMN UserInfoUserId TO UserId;");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_UserId",
                table: "ToDoList",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_UserInfo_UserId",
                table: "ToDoList",
                column: "UserId",
                principalTable: "UserInfo",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_UserInfo_UserId",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_UserId",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDoList");

            migrationBuilder.AddColumn<int>(
                name: "UserInfoUserId",
                table: "ToDoList",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoList_UserInfoUserId",
                table: "ToDoList",
                column: "UserInfoUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_UserInfo_UserInfoUserId",
                table: "ToDoList",
                column: "UserInfoUserId",
                principalTable: "UserInfo",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
