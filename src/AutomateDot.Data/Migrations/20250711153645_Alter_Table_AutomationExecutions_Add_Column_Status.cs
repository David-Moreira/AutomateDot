using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomateDot.Data.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Table_AutomationExecutions_Add_Column_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AutomationExecutions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AutomationExecutions");
        }
    }
}
