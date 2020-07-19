using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TranscripTrack.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrencyCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    CurrencyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profiles_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineRates",
                columns: table => new
                {
                    LineRateId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineRates", x => x.LineRateId);
                    table.ForeignKey(
                        name: "FK_LineRates_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineRateEntries",
                columns: table => new
                {
                    LineRateEntryId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumLines = table.Column<int>(nullable: false),
                    EnteredDate = table.Column<DateTime>(nullable: false),
                    LineRateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineRateEntries", x => x.LineRateEntryId);
                    table.ForeignKey(
                        name: "FK_LineRateEntries_LineRates_LineRateId",
                        column: x => x.LineRateId,
                        principalTable: "LineRates",
                        principalColumn: "LineRateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineRateEntries_LineRateId",
                table: "LineRateEntries",
                column: "LineRateId");

            migrationBuilder.CreateIndex(
                name: "IX_LineRates_ProfileId",
                table: "LineRates",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CurrencyId",
                table: "Profiles",
                column: "CurrencyId");

            migrationBuilder.InsertData(
                "Currencies",
                "CurrencyCode",
                new object[] { "AUD", "CAD", "GBP", "NZD", "USD" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineRateEntries");

            migrationBuilder.DropTable(
                name: "LineRates");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
