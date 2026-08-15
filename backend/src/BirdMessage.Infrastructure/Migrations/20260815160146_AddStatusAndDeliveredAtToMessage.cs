using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirdMessage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAndDeliveredAtToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "delivered_at",
                table: "messages",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "delivered_at",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "status",
                table: "messages");
        }
    }
}
