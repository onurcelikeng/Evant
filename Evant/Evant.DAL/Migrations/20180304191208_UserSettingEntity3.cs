using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Evant.DAL.Migrations
{
    public partial class UserSettingEntity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsJoinEventVisiableTimeline",
                table: "UserSettings",
                newName: "IsJoinEventVisibleTimeline");

            migrationBuilder.RenameColumn(
                name: "IsFollowingVisiableTimeline",
                table: "UserSettings",
                newName: "IsFollowingVisibleTimeline");

            migrationBuilder.RenameColumn(
                name: "IsFollowerVisiableTimeline",
                table: "UserSettings",
                newName: "IsFollowerVisibleTimeline");

            migrationBuilder.RenameColumn(
                name: "IsCreateEventVisiableTimeline",
                table: "UserSettings",
                newName: "IsCreateEventVisibleTimeline");

            migrationBuilder.RenameColumn(
                name: "IsCommentVisiableTimeline",
                table: "UserSettings",
                newName: "IsCommentVisibleTimeline");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsJoinEventVisibleTimeline",
                table: "UserSettings",
                newName: "IsJoinEventVisiableTimeline");

            migrationBuilder.RenameColumn(
                name: "IsFollowingVisibleTimeline",
                table: "UserSettings",
                newName: "IsFollowingVisiableTimeline");

            migrationBuilder.RenameColumn(
                name: "IsFollowerVisibleTimeline",
                table: "UserSettings",
                newName: "IsFollowerVisiableTimeline");

            migrationBuilder.RenameColumn(
                name: "IsCreateEventVisibleTimeline",
                table: "UserSettings",
                newName: "IsCreateEventVisiableTimeline");

            migrationBuilder.RenameColumn(
                name: "IsCommentVisibleTimeline",
                table: "UserSettings",
                newName: "IsCommentVisiableTimeline");
        }
    }
}
