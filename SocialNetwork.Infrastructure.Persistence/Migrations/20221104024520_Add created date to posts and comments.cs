using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Infrastructure.Persistence.Migrations
{
    public partial class Addcreateddatetopostsandcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Friends_FriendsId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_FriendsId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "FriendsId",
                table: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FriendsId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_FriendsId",
                table: "Posts",
                column: "FriendsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Friends_FriendsId",
                table: "Posts",
                column: "FriendsId",
                principalTable: "Friends",
                principalColumn: "Id");
        }
    }
}
