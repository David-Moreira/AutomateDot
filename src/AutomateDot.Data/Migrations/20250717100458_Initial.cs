using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomateDot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutomateRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Trigger = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    TriggerConfiguration = table.Column<string>(type: "TEXT", nullable: false),
                    ActionConfiguration = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomateRecipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutomateExecutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AutomateRecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Trigger = table.Column<string>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomateExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomateExecutions_AutomateRecipes_AutomateRecipeId",
                        column: x => x.AutomateRecipeId,
                        principalTable: "AutomateRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomateExecutionEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AutomateExecutionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomateExecutionEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomateExecutionEntries_AutomateExecutions_AutomateExecutionId",
                        column: x => x.AutomateExecutionId,
                        principalTable: "AutomateExecutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutomateExecutionEntries_AutomateExecutionId",
                table: "AutomateExecutionEntries",
                column: "AutomateExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomateExecutions_AutomateRecipeId",
                table: "AutomateExecutions",
                column: "AutomateRecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomateExecutionEntries");

            migrationBuilder.DropTable(
                name: "AutomateExecutions");

            migrationBuilder.DropTable(
                name: "AutomateRecipes");
        }
    }
}
