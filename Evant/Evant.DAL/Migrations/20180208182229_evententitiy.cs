using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Evant.DAL.Migrations
{
    public partial class evententitiy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Addresses_Id",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "Addresses",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Addresses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Addresses",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "Addresses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_EventId",
                table: "Addresses",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Events_EventId",
                table: "Addresses",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Events_EventId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_EventId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Addresses",
                newName: "AddressId");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Events",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Addresses_Id",
                table: "Events",
                column: "Id",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
