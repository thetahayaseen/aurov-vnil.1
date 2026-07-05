using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mark_vnil._1.Migrations
{
    /// <inheritdoc />
    public partial class IndexAddedOnROVDetectedItemLabel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ROVDetectedItems_Label",
                table: "ROVDetectedItems",
                column: "Label");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ROVDetectedItems_Label",
                table: "ROVDetectedItems");
        }
    }
}
