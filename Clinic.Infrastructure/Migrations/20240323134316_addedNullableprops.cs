using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedNullableprops : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Schedule_ScheduleID",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Speciality_SpecialityID",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Paycard_PaycardID",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ScheduleID",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "PaycardID",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialityID",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleID",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Schedule_ScheduleID",
                table: "Doctors",
                column: "ScheduleID",
                principalTable: "Schedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Speciality_SpecialityID",
                table: "Doctors",
                column: "SpecialityID",
                principalTable: "Speciality",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Paycard_PaycardID",
                table: "Patients",
                column: "PaycardID",
                principalTable: "Paycard",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Schedule_ScheduleID",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Speciality_SpecialityID",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Paycard_PaycardID",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "PaycardID",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleID",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SpecialityID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleID",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Schedule_ScheduleID",
                table: "Doctors",
                column: "ScheduleID",
                principalTable: "Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Speciality_SpecialityID",
                table: "Doctors",
                column: "SpecialityID",
                principalTable: "Speciality",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Paycard_PaycardID",
                table: "Patients",
                column: "PaycardID",
                principalTable: "Paycard",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
