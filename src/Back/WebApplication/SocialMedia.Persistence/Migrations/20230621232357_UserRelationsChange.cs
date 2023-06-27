using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserRelationsChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRelation_AspNetUsers_FollowerId",
                table: "UserRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRelation_AspNetUsers_UserId",
                table: "UserRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRelation",
                table: "UserRelation");

            migrationBuilder.RenameTable(
                name: "UserRelation",
                newName: "UserRelations");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "UserRelations",
                newName: "FollowingId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRelation_UserId",
                table: "UserRelations",
                newName: "IX_UserRelations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRelation_FollowerId",
                table: "UserRelations",
                newName: "IX_UserRelations_FollowingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRelations",
                table: "UserRelations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRelations_AspNetUsers_FollowingId",
                table: "UserRelations",
                column: "FollowingId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRelations_AspNetUsers_UserId",
                table: "UserRelations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRelations_AspNetUsers_FollowingId",
                table: "UserRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRelations_AspNetUsers_UserId",
                table: "UserRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRelations",
                table: "UserRelations");

            migrationBuilder.RenameTable(
                name: "UserRelations",
                newName: "UserRelation");

            migrationBuilder.RenameColumn(
                name: "FollowingId",
                table: "UserRelation",
                newName: "FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRelations_UserId",
                table: "UserRelation",
                newName: "IX_UserRelation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRelations_FollowingId",
                table: "UserRelation",
                newName: "IX_UserRelation_FollowerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRelation",
                table: "UserRelation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRelation_AspNetUsers_FollowerId",
                table: "UserRelation",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRelation_AspNetUsers_UserId",
                table: "UserRelation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
