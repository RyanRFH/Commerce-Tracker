using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace commerce_tracker_v2.Migrations
{
    /// <inheritdoc />
    public partial class AddingBaskets2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6187b28-c364-47f7-8de5-9cfa8175186b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9f6fa05-7195-4ce7-937b-0bb1262104af");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "337015da-07b9-453c-8e80-1caf97d64d75", null, "User", "USER" },
                    { "bd19ded9-6d83-4411-a58b-eab69d01cdf4", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "337015da-07b9-453c-8e80-1caf97d64d75");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd19ded9-6d83-4411-a58b-eab69d01cdf4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b6187b28-c364-47f7-8de5-9cfa8175186b", null, "Admin", "ADMIN" },
                    { "c9f6fa05-7195-4ce7-937b-0bb1262104af", null, "User", "USER" }
                });
        }
    }
}
