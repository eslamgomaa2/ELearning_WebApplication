using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNewColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "InAppNotification",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EmailNotification",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CourseEnrollmentConfirmation",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmergencyAlerts",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnrollmentCapacityAlert",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExamSubmissionEmail",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExamSubmissionInApp",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "FailingGradeWarning",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GradePublished",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LowAttendanceAlert",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NewStudentEnrollmentEmail",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "NewStudentEnrollmentInApp",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "QuizSubmission",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WeeklyActivityDigest",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "InstructorProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AcademicSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowInstructorsToCreateCourses = table.Column<bool>(type: "bit", nullable: false),
                    MaxCourseDurationWeeks = table.Column<int>(type: "int", nullable: false),
                    MinCourseDurationWeeks = table.Column<int>(type: "int", nullable: false),
                    AutoPublishResults = table.Column<bool>(type: "bit", nullable: false),
                    ResultReleaseDelayDays = table.Column<int>(type: "int", nullable: false),
                    GradeAppealPeriodDays = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeRanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademicSettingId = table.Column<int>(type: "int", nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinScore = table.Column<int>(type: "int", nullable: false),
                    MaxScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeRanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeRanges_AcademicSettings_AcademicSettingId",
                        column: x => x.AcademicSettingId,
                        principalTable: "AcademicSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradeRanges_AcademicSettingId",
                table: "GradeRanges",
                column: "AcademicSettingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeRanges");

            migrationBuilder.DropTable(
                name: "AcademicSettings");

            migrationBuilder.DropColumn(
                name: "CourseEnrollmentConfirmation",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "EmergencyAlerts",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "EnrollmentCapacityAlert",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "ExamSubmissionEmail",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "ExamSubmissionInApp",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "FailingGradeWarning",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "GradePublished",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "LowAttendanceAlert",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "NewStudentEnrollmentEmail",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "NewStudentEnrollmentInApp",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "QuizSubmission",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "WeeklyActivityDigest",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "InstructorProfiles");

            migrationBuilder.AlterColumn<bool>(
                name: "InAppNotification",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailNotification",
                table: "NotificationSettings",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
