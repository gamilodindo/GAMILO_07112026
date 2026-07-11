using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileProcessor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessedFileDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    FileType = table.Column<int>(type: "INTEGER", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", nullable: false),
                    FileSize = table.Column<double>(type: "REAL", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Result = table.Column<string>(type: "TEXT", nullable: false),
                    RecordCount = table.Column<int>(type: "INTEGER", nullable: false),
                    ProcessingTime = table.Column<double>(type: "REAL", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedFileDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedFileDetails");
        }
    }
}
