using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieDB.Api.Migrations
{
    public partial class AddMoviePosterUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Artist",
                table: "Concerts",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "PosterUrl",
                table: "Movies",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterUrl",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Concerts",
                newName: "Artist");
        }
    }
}
