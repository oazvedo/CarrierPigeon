using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirdMessage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberToAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "number",
                table: "addresses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "number",
                table: "addresses");
        }
    }
}
