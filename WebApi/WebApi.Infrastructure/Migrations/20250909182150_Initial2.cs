using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Triages_PatientId",
                table: "Triages");

            migrationBuilder.CreateIndex(
                name: "IX_Triages_PatientId",
                table: "Triages",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Triages_PatientId",
                table: "Triages");

            migrationBuilder.CreateIndex(
                name: "IX_Triages_PatientId",
                table: "Triages",
                column: "PatientId",
                unique: true);
        }
    }
}
