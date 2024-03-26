using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class patIDuniquePaycard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Paycard_PatientID",
                table: "Paycard");

            migrationBuilder.CreateIndex(
                name: "IX_Paycard_PatientID",
                table: "Paycard",
                column: "PatientID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Paycard_PatientID",
                table: "Paycard");

            migrationBuilder.CreateIndex(
                name: "IX_Paycard_PatientID",
                table: "Paycard",
                column: "PatientID");
        }
    }
}
