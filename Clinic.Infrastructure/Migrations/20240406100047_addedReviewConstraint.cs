using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedReviewConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_DoctorID",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DoctorID_PatientID_date",
                table: "Reviews",
                columns: new[] { "DoctorID", "PatientID", "date" },
                unique: true,
                filter: "[date] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_DoctorID_PatientID_date",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DoctorID",
                table: "Reviews",
                column: "DoctorID");
        }
    }
}
