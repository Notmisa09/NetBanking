using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBanking.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class ImageURLfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                schema: "Identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                schema: "Identity",
                table: "Users");
        }
    }
}
