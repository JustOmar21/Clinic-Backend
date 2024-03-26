using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedDateUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointements_PatientID",
                table: "Appointements");

            migrationBuilder.CreateIndex(
                name: "IX_Appointements_PatientID_DoctorID_Date",
                table: "Appointements",
                columns: new[] { "PatientID", "DoctorID", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointements_PatientID_DoctorID_Date",
                table: "Appointements");

            migrationBuilder.CreateIndex(
                name: "IX_Appointements_PatientID",
                table: "Appointements",
                column: "PatientID");
        }
    }
}
