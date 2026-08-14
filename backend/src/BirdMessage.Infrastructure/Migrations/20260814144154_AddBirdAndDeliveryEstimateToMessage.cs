using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BirdMessage.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBirdAndDeliveryEstimateToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "bird_id",
                table: "messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "distance",
                table: "messages",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "estimated_delivery_minutes",
                table: "messages",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bird_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "distance",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "estimated_delivery_minutes",
                table: "messages");
        }
    }
}
