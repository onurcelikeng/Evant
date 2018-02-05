using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Evant.DAL.Migrations
{
    public partial class usersetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserSettingId = table.Column<Guid>(nullable: false),
                    IsCommentNotif = table.Column<bool>(type: "bit", nullable: false),
                    IsEventNewComerNotif = table.Column<bool>(type: "bit", nullable: false),
                    IsEventUpdateNotif = table.Column<bool>(type: "bit", nullable: false),
                    IsFriendshipNotif = table.Column<bool>(type: "bit", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserSettingId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserSettings_Id",
                table: "Users",
                column: "Id",
                principalTable: "UserSettings",
                principalColumn: "UserSettingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSettings_Id",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
