using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieDB.Api.Migrations
{
    public partial class updateRefreshTokenRevokedColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Revoked",
                table: "RefreshToken",
                newName: "RevokedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                table: "RefreshToken",
                newName: "Revoked");
        }
    }
}
