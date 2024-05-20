using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hal.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_CATEGORY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CategoryName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    SubCategoryName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CATEGORY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_EVALUATION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Sentiment = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_EVALUATION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PasswordHash = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    UserEmail = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DateRegister = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    IsActive = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_COMMENTS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CommentText = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    UserId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    EvaluationId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CategoryId = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_COMMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_COMMENTS_TB_CATEGORY_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TB_CATEGORY",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_COMMENTS_TB_EVALUATION_EvaluationId",
                        column: x => x.EvaluationId,
                        principalTable: "TB_EVALUATION",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TB_COMMENTS_TB_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMMENTS_CategoryId",
                table: "TB_COMMENTS",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMMENTS_EvaluationId",
                table: "TB_COMMENTS",
                column: "EvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_COMMENTS_UserId",
                table: "TB_COMMENTS",
                column: "UserId");
            // Inserir dados padrão na tabela TB_CATEGORY
            migrationBuilder.InsertData(
                table: "TB_CATEGORY",
                columns: new[] { "Id", "CategoryName", "SubCategoryName" },
                values: new object[] { 1, "Default Category", "Default SubCategory" });

            // Inserir dados padrão na tabela TB_EVALUATION
            migrationBuilder.InsertData(
                table: "TB_EVALUATION",
                columns: new[] { "Id", "Sentiment" },
                values: new object[] { 1, "Neutral" });

            // Inserir dados padrão na tabela TB_EVALUATION
            migrationBuilder.InsertData(
                table: "TB_EVALUATION",
                columns: new[] { "Id", "Sentiment" },
                values: new object[] { 2, "Positive" });

            // Inserir dados padrão na tabela TB_EVALUATION
            migrationBuilder.InsertData(
                table: "TB_EVALUATION",
                columns: new[] { "Id", "Sentiment" },
                values: new object[] { 3, "Negative" });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_COMMENTS");

            migrationBuilder.DropTable(
                name: "TB_CATEGORY");

            migrationBuilder.DropTable(
                name: "TB_EVALUATION");

            migrationBuilder.DropTable(
                name: "TB_USER");
        }
    }
}
