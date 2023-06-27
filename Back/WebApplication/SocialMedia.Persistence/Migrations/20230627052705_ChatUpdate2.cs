using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChatUpdate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignalrId",
                table: "ChatConnections");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "ChatConnections");

            migrationBuilder.AddColumn<int>(
                name: "SecondUserId",
                table: "ChatConnections",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondUserId",
                table: "ChatConnections");

            migrationBuilder.AddColumn<string>(
                name: "SignalrId",
                table: "ChatConnections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "ChatConnections",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
