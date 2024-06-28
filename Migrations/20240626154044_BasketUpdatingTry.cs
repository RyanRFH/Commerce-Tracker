using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace commerce_tracker_v2.Migrations
{
    /// <inheritdoc />
    public partial class BasketUpdatingTry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "0e763ee6-ccfd-4edb-ac16-cd2425234137", null, "Admin", "ADMIN" },
                    { "8bc8b0ce-dfec-4f39-b4c3-4ecc94b7a372", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e763ee6-ccfd-4edb-ac16-cd2425234137");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8bc8b0ce-dfec-4f39-b4c3-4ecc94b7a372");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "337015da-07b9-453c-8e80-1caf97d64d75", null, "User", "USER" },
                    { "bd19ded9-6d83-4411-a58b-eab69d01cdf4", null, "Admin", "ADMIN" }
                });
        }
    }
}
