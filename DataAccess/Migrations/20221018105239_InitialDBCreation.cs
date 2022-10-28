using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialDBCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "MidExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Table = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    No = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeExchangeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Table = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    No = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MidRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mid = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    MidExchangeRatesId = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MidRates_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MidRates_MidExchangeRates_MidExchangeRatesId",
                        column: x => x.MidExchangeRatesId,
                        principalTable: "MidExchangeRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TradeRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bid = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    Ask = table.Column<decimal>(type: "decimal(18,10)", nullable: false),
                    TradeExchangeRatesId = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "varchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeRates_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TradeRates_TradeExchangeRates_TradeExchangeRatesId",
                        column: x => x.TradeExchangeRatesId,
                        principalTable: "TradeExchangeRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MidRates_CurrencyCode",
                table: "MidRates",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_MidRates_MidExchangeRatesId",
                table: "MidRates",
                column: "MidExchangeRatesId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRates_CurrencyCode",
                table: "TradeRates",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_TradeRates_TradeExchangeRatesId",
                table: "TradeRates",
                column: "TradeExchangeRatesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MidRates");

            migrationBuilder.DropTable(
                name: "TradeRates");

            migrationBuilder.DropTable(
                name: "MidExchangeRates");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "TradeExchangeRates");
        }
    }
}
