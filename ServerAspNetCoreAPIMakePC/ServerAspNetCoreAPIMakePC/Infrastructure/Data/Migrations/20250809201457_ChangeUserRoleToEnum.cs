using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerAspNetCoreAPIMakePC.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserRoleToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add a temporary int column to store the enum value
            migrationBuilder.AddColumn<int>(
                name: "RoleTemp",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            // Step 2: Copy values from string Role to int RoleTemp
            migrationBuilder.Sql(
                @"UPDATE Users SET RoleTemp =
            CASE
                WHEN Role = 'User' THEN 0
                WHEN Role = 'Admin' THEN 1
                WHEN Role = 'Moderator' THEN 2
                WHEN Role = 'Guest' THEN 3
                ELSE 0
            END"
            );

            // Step 3: Drop the old string column
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            // Step 4: Rename the temp column to Role
            migrationBuilder.RenameColumn(
                name: "RoleTemp",
                table: "Users",
                newName: "Role"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Role of the user in the system (e.g., Admin, User).",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 30,
                oldComment: "Role of the user in the system (e.g., Admin, User).");
        }
    }
}
