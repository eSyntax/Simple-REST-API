using Microsoft.EntityFrameworkCore.Migrations;

namespace Simple_API.Migrations
{
    public partial class AddtodolistUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE UserInfo CHANGE COLUMN UserName Email VARCHAR(30);");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserInfo",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_UserInfo_UserInfoUserId",
                table: "ToDoList");

            migrationBuilder.DropIndex(
                name: "IX_ToDoList_UserInfoUserId",
                table: "ToDoList");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserInfo");

            migrationBuilder.DropColumn(
                name: "UserInfoUserId",
                table: "ToDoList");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserInfo",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserInfo",
                type: "varchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
