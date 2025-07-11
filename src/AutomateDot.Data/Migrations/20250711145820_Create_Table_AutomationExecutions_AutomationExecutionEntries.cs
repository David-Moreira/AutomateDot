using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomateDot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create_Table_AutomationExecutions_AutomationExecutionEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutomationExecutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AutomationRecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TriggerType = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionType = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomationExecutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomationExecutions_AutomationRecipes_AutomationRecipeId",
                        column: x => x.AutomationRecipeId,
                        principalTable: "AutomationRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomationExecutionEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AutomationExecutionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomationExecutionEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomationExecutionEntries_AutomationExecutions_AutomationExecutionId",
                        column: x => x.AutomationExecutionId,
                        principalTable: "AutomationExecutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutomationExecutionEntries_AutomationExecutionId",
                table: "AutomationExecutionEntries",
                column: "AutomationExecutionId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationExecutions_AutomationRecipeId",
                table: "AutomationExecutions",
                column: "AutomationRecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutomationExecutionEntries");

            migrationBuilder.DropTable(
                name: "AutomationExecutions");
        }
    }
}
