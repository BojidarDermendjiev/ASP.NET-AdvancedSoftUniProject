using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerAspNetCoreAPIMakePC.Migrations
{
    /// <inheritdoc />
    public partial class SeedDBAndModifiedUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarImage",
                table: "Users",
                type: "varbinary(max)",
                nullable: true,
                comment: "Avatar image for the user, stored as byte array.");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Users",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                comment: "URL or file path to the user's avatar image.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarImage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Users");
        }
    }
}
