using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vitacore.Test.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddedTangerineLotAndBidEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tangerine_lots",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    image_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    start_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    current_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    buyout_price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    auction_start_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    auction_end_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    current_leader_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    purchase_type = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tangerine_lots", x => x.id);
                    table.CheckConstraint("ck_tangerine_lots_auction_window_valid", "\"auction_end_at\" > \"auction_start_at\"");
                    table.CheckConstraint("ck_tangerine_lots_buyout_price_valid", "\"buyout_price\" IS NULL OR \"buyout_price\" >= \"start_price\"");
                    table.CheckConstraint("ck_tangerine_lots_current_price_valid", "\"current_price\" >= \"start_price\"");
                    table.CheckConstraint("ck_tangerine_lots_expiration_valid", "\"expiration_at\" >= \"auction_end_at\"");
                    table.CheckConstraint("ck_tangerine_lots_purchase_consistency", "(\"purchase_type\" IS NULL AND \"buyer_id\" IS NULL) OR (\"purchase_type\" IS NOT NULL AND \"buyer_id\" IS NOT NULL)");
                    table.CheckConstraint("ck_tangerine_lots_start_price_positive", "\"start_price\" > 0");
                    table.ForeignKey(
                        name: "fk_tangerine_lots_asp_net_users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tangerine_lots_asp_net_users_current_leader_user_id",
                        column: x => x.current_leader_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bids",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    lot_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bids", x => x.id);
                    table.CheckConstraint("ck_bids_amount_positive", "\"amount\" > 0");
                    table.ForeignKey(
                        name: "fk_bids_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_bids_tangerine_lots_lot_id",
                        column: x => x.lot_id,
                        principalTable: "tangerine_lots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bids_lot_id",
                table: "bids",
                column: "lot_id");

            migrationBuilder.CreateIndex(
                name: "ix_bids_user_id",
                table: "bids",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tangerine_lots_buyer_id",
                table: "tangerine_lots",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "ix_tangerine_lots_current_leader_user_id",
                table: "tangerine_lots",
                column: "current_leader_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tangerine_lots_expiration_at",
                table: "tangerine_lots",
                column: "expiration_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bids");

            migrationBuilder.DropTable(
                name: "tangerine_lots");
        }
    }
}
