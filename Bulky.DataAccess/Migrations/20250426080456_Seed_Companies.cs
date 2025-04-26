using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BulkyBook.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Companies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "San Francisco", "Tech Solutions", "415-123-4567", "94105", "CA", "123 Tech Street" },
                    { 2, "Austin", "Health Plus", "512-987-6543", "73301", "TX", "456 Wellness Blvd" },
                    { 3, "Denver", "Green Energy", "303-456-7890", "80014", "CO", "789 Solar Drive" },
                    { 4, "Boston", "EduSmart", "617-234-5678", "02108", "MA", "321 Knowledge Ave" },
                    { 5, "New York", "FinCorp", "212-345-6789", "10001", "NY", "654 Finance Road" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
