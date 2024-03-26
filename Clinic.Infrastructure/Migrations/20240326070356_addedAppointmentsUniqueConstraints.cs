using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedAppointmentsUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointements_Date_Order",
                table: "Appointements");

            migrationBuilder.DropIndex(
                name: "IX_Appointements_PatientID_DoctorID_Date",
                table: "Appointements");

            migrationBuilder.CreateIndex(
                name: "IX_Appointements_Date_Order",
                table: "Appointements",
                columns: new[] { "Date", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointements_PatientID_DoctorID_Date",
                table: "Appointements",
                columns: new[] { "PatientID", "DoctorID", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointements_Date_Order",
                table: "Appointements");

            migrationBuilder.DropIndex(
                name: "IX_Appointements_PatientID_DoctorID_Date",
                table: "Appointements");

            migrationBuilder.CreateIndex(
                name: "IX_Appointements_Date_Order",
                table: "Appointements",
                columns: new[] { "Date", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointements_PatientID_DoctorID_Date",
                table: "Appointements",
                columns: new[] { "PatientID", "DoctorID", "Date" });
        }
    }
}
