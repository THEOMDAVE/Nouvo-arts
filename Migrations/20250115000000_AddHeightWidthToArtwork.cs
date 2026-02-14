using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NouvoStudio.Migrations
{
    /// <inheritdoc />
    public partial class AddHeightWidthToArtwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeightFeet",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WidthFeet",
                table: "Artworks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightFeet",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "WidthFeet",
                table: "Artworks");
        }
    }
}

