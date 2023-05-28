using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Online_Marketplace.DAL.Migrations
{
    /// <inheritdoc />
    public partial class forms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60703d4a-3be5-4e56-8dc2-17c00a516b09");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a6622e8-55e3-400b-9b77-94d2016b27ed");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f515e8a9-5e43-476e-b72e-42c93c8503b6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ded9550-6cb0-4244-a446-bc4f5bfc5e0e", "3fdbd41f-c59f-47db-9e26-02bd6671b0e6", "Buyer", "BUYER" },
                    { "cb2fb31c-b90a-480c-a9cb-aae45674231f", "65d39126-0da2-41b6-928e-daccdceeee97", "Seller", "SELLER" },
                    { "f412f719-7c1e-49dc-94a1-85e8c9805f48", "4fdf1b90-016b-47cb-a243-8389b985e800", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ded9550-6cb0-4244-a446-bc4f5bfc5e0e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb2fb31c-b90a-480c-a9cb-aae45674231f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f412f719-7c1e-49dc-94a1-85e8c9805f48");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60703d4a-3be5-4e56-8dc2-17c00a516b09", "f0c96c75-ed78-4ca0-b404-c3ec5b5598c0", "Admin", "ADMIN" },
                    { "6a6622e8-55e3-400b-9b77-94d2016b27ed", "20e0acc5-f04b-40d4-9d51-c90d4790369c", "Buyer", "BUYER" },
                    { "f515e8a9-5e43-476e-b72e-42c93c8503b6", "849dfb1e-fcd2-4094-9822-317d863d83d8", "Seller", "SELLER" }
                });
        }
    }
}
