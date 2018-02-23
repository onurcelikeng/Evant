using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Evant.DAL.Migrations
{
    public partial class userupdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFactorAuthentication",
                table: "UserSettings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 140);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTwoFactorAuthentication",
                table: "UserSettings");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                maxLength: 140,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
