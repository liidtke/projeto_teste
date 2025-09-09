using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Triages_Patients_PatientArrivalId",
                table: "Triages");

            migrationBuilder.AddForeignKey(
                name: "FK_Triages_PatientArrivals_PatientArrivalId",
                table: "Triages",
                column: "PatientArrivalId",
                principalTable: "PatientArrivals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Triages_PatientArrivals_PatientArrivalId",
                table: "Triages");

            migrationBuilder.AddForeignKey(
                name: "FK_Triages_Patients_PatientArrivalId",
                table: "Triages",
                column: "PatientArrivalId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
