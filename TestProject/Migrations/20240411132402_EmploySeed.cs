using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestProject.Migrations
{
    /// <inheritdoc />
    public partial class EmploySeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employs",
                columns: new[] { "Id", "Age", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, " ", "franco" },
                    { 2, null, " ", "carlo" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employs",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
