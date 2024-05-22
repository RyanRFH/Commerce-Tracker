using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace commerce_tracker_v2.Migrations
{
    /// <inheritdoc />
    public partial class FirstMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c0bb83f6-6154-40b3-9400-a53caabaa934");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddd50357-4736-4d70-bfac-adb6b0c19ef5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e798b893-9867-4617-bd8d-dd426f3a0962");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "016f12da-be70-45aa-88a7-75be98ab415d", null, "Admin", "ADMIN" },
                    { "7832f61e-bca1-4da0-bda3-5c9f944cc235", null, "Test", "TEST" },
                    { "cb8ac4ee-beca-48ea-bced-6a62b266b8f9", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "016f12da-be70-45aa-88a7-75be98ab415d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7832f61e-bca1-4da0-bda3-5c9f944cc235");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb8ac4ee-beca-48ea-bced-6a62b266b8f9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "c0bb83f6-6154-40b3-9400-a53caabaa934", null, "Test", "TEST" },
                    { "ddd50357-4736-4d70-bfac-adb6b0c19ef5", null, "Admin", "ADMIN" },
                    { "e798b893-9867-4617-bd8d-dd426f3a0962", null, "User", "USER" }
                });
        }
    }
}
