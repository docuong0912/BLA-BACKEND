using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Draft15.Migrations
{
    /// <inheritdoc />
    public partial class initialdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    assignment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    assignment_title = table.Column<string>(type: "text", nullable: false),
                    assignment_description = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    attemp = table.Column<int>(type: "integer", nullable: true),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.assignment_id);
                });

            migrationBuilder.CreateTable(
                name: "Content",
                columns: table => new
                {
                    content_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    isFinish = table.Column<bool>(type: "boolean", nullable: false),
                    prerequisitecontent_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Content", x => x.content_id);
                    table.ForeignKey(
                        name: "FK_Content_Content_prerequisitecontent_id",
                        column: x => x.prerequisitecontent_id,
                        principalTable: "Content",
                        principalColumn: "content_id");
                });

            migrationBuilder.CreateTable(
                name: "Submission",
                columns: table => new
                {
                    submission_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    submission_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    assignment_id = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true),
                    grade = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submission", x => x.submission_id);
                    table.ForeignKey(
                        name: "FK_Submission_Assignment_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "Assignment",
                        principalColumn: "assignment_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    file_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    file_type = table.Column<string>(type: "text", nullable: false),
                    file_path = table.Column<string>(type: "text", nullable: true),
                    uploadDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    assignment_id = table.Column<int>(type: "integer", nullable: true),
                    content_id = table.Column<int>(type: "integer", nullable: true),
                    submission_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.file_id);
                    table.ForeignKey(
                        name: "FK_File_Assignment_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "Assignment",
                        principalColumn: "assignment_id");
                    table.ForeignKey(
                        name: "FK_File_Content_content_id",
                        column: x => x.content_id,
                        principalTable: "Content",
                        principalColumn: "content_id");
                    table.ForeignKey(
                        name: "FK_File_Submission_submission_id",
                        column: x => x.submission_id,
                        principalTable: "Submission",
                        principalColumn: "submission_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Content_prerequisitecontent_id",
                table: "Content",
                column: "prerequisitecontent_id");

            migrationBuilder.CreateIndex(
                name: "IX_File_assignment_id",
                table: "File",
                column: "assignment_id");

            migrationBuilder.CreateIndex(
                name: "IX_File_content_id",
                table: "File",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_File_submission_id",
                table: "File",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_Submission_assignment_id",
                table: "Submission",
                column: "assignment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Content");

            migrationBuilder.DropTable(
                name: "Submission");

            migrationBuilder.DropTable(
                name: "Assignment");
        }
    }
}
