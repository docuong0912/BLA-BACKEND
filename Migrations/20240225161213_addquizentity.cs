using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Draft15.Migrations
{
    /// <inheritdoc />
    public partial class addquizentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    quiz_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quiz_title = table.Column<string>(type: "text", nullable: false),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    content_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.quiz_id);
                    table.ForeignKey(
                        name: "FK_Quiz_Content_content_id",
                        column: x => x.content_id,
                        principalTable: "Content",
                        principalColumn: "content_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempt",
                columns: table => new
                {
                    attempt_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    quiz_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempt", x => x.attempt_id);
                    table.ForeignKey(
                        name: "FK_QuizAttempt_Quiz_quiz_id",
                        column: x => x.quiz_id,
                        principalTable: "Quiz",
                        principalColumn: "quiz_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestion",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quesion_content = table.Column<string>(type: "text", nullable: false),
                    quiz_id = table.Column<int>(type: "integer", nullable: false),
                    quesion_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestion", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_QuizQuestion_Quiz_quiz_id",
                        column: x => x.quiz_id,
                        principalTable: "Quiz",
                        principalColumn: "quiz_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizOption",
                columns: table => new
                {
                    option_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    quesion_id = table.Column<int>(type: "integer", nullable: false),
                    question_id = table.Column<int>(type: "integer", nullable: true),
                    isCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    option_text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizOption", x => x.option_id);
                    table.ForeignKey(
                        name: "FK_QuizOption_QuizQuestion_question_id",
                        column: x => x.question_id,
                        principalTable: "QuizQuestion",
                        principalColumn: "question_id");
                });

            migrationBuilder.CreateTable(
                name: "QuizResponse",
                columns: table => new
                {
                    response_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    attempt_id = table.Column<int>(type: "integer", nullable: false),
                    question_id = table.Column<int>(type: "integer", nullable: false),
                    choosenoption_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizResponse", x => x.response_id);
                    table.ForeignKey(
                        name: "FK_QuizResponse_QuizAttempt_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "QuizAttempt",
                        principalColumn: "attempt_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizResponse_QuizOption_choosenoption_id",
                        column: x => x.choosenoption_id,
                        principalTable: "QuizOption",
                        principalColumn: "option_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizResponse_QuizQuestion_question_id",
                        column: x => x.question_id,
                        principalTable: "QuizQuestion",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_content_id",
                table: "Quiz",
                column: "content_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempt_quiz_id",
                table: "QuizAttempt",
                column: "quiz_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizOption_question_id",
                table: "QuizOption",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestion_quiz_id",
                table: "QuizQuestion",
                column: "quiz_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizResponse_attempt_id",
                table: "QuizResponse",
                column: "attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizResponse_choosenoption_id",
                table: "QuizResponse",
                column: "choosenoption_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuizResponse_question_id",
                table: "QuizResponse",
                column: "question_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizResponse");

            migrationBuilder.DropTable(
                name: "QuizAttempt");

            migrationBuilder.DropTable(
                name: "QuizOption");

            migrationBuilder.DropTable(
                name: "QuizQuestion");

            migrationBuilder.DropTable(
                name: "Quiz");
        }
    }
}
