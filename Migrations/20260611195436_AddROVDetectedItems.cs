using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mark_vnil._1.Migrations
{
    /// <inheritdoc />
    public partial class AddROVDetectedItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ROVDetectedItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StreamId = table.Column<int>(type: "INTEGER", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    Confidence = table.Column<float>(type: "REAL", nullable: false),
                    DetectedAtTimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SnapshotFileUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROVDetectedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ROVDetectedItems_ROVStreams_StreamId",
                        column: x => x.StreamId,
                        principalTable: "ROVStreams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ROVDetectedItems_StreamId",
                table: "ROVDetectedItems",
                column: "StreamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROVDetectedItems");
        }
    }
}
