using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieDB.Api.Migrations
{
    public partial class updateAccountsColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Verified",
                table: "Accounts",
                newName: "VerifiedAt");

            migrationBuilder.RenameColumn(
                name: "ResetTokenExpires",
                table: "Accounts",
                newName: "ResetTokenExpiresAt");

            migrationBuilder.RenameColumn(
                name: "PasswordReset",
                table: "Accounts",
                newName: "PasswordResetAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerifiedAt",
                table: "Accounts",
                newName: "Verified");

            migrationBuilder.RenameColumn(
                name: "ResetTokenExpiresAt",
                table: "Accounts",
                newName: "ResetTokenExpires");

            migrationBuilder.RenameColumn(
                name: "PasswordResetAt",
                table: "Accounts",
                newName: "PasswordReset");
        }
    }
}
