using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiedReviewandAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rejection",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "Rejection",
                table: "Appointements",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rejection",
                table: "Appointements");

            migrationBuilder.AddColumn<string>(
                name: "rejection",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
