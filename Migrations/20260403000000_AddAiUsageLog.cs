using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrixCMS.Open.Migrations
{
    /// <inheritdoc />
    public partial class AddAiUsageLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AiUsageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Operation = table.Column<string>(type: "TEXT", nullable: false),
                    Provider  = table.Column<string>(type: "TEXT", nullable: false),
                    Model     = table.Column<string>(type: "TEXT", nullable: false),
                    InputTokens  = table.Column<int>(type: "INTEGER", nullable: false),
                    OutputTokens = table.Column<int>(type: "INTEGER", nullable: false),
                    EstimatedCostUsd = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiUsageLogs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "AiUsageLogs");
        }
    }
}
