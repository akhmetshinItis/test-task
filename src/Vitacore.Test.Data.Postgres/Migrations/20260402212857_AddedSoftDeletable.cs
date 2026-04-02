using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vitacore.Test.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddedSoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "tangerine_lots",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "tangerine_lots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "ix_tangerine_lots_is_deleted_expiration_at",
                table: "tangerine_lots",
                columns: new[] { "is_deleted", "expiration_at" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tangerine_lots_is_deleted_expiration_at",
                table: "tangerine_lots");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "tangerine_lots");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "tangerine_lots");
        }
    }
}
