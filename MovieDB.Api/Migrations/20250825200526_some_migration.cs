using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDB.Api.App.Migrations
{
    /// <inheritdoc />
    public partial class some_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concerts_Accounts_AccountId",
                table: "Concerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Accounts_AccountId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Theaters_Accounts_AccountId",
                table: "Theaters");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Theaters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Movies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Concerts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Concerts_Accounts_AccountId",
                table: "Concerts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Accounts_AccountId",
                table: "Movies",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Theaters_Accounts_AccountId",
                table: "Theaters",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Concerts_Accounts_AccountId",
                table: "Concerts");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Accounts_AccountId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Theaters_Accounts_AccountId",
                table: "Theaters");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Theaters",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Movies",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Concerts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Concerts_Accounts_AccountId",
                table: "Concerts",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Accounts_AccountId",
                table: "Movies",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Theaters_Accounts_AccountId",
                table: "Theaters",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
