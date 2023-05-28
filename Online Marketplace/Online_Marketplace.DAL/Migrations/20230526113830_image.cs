using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Online_Marketplace.DAL.Migrations
{
    /// <inheritdoc />
    public partial class image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f77e37b-704c-42d0-ae59-3e0f624df6bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33d3a537-6d65-4f34-9f5c-2931d6d98115");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a46b133f-912a-4e09-a0dc-e215803b23b6");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f77e37b-704c-42d0-ae59-3e0f624df6bc", "86accf09-b6cd-4119-ac2a-a2bb6336ff49", "Seller", "SELLER" },
                    { "33d3a537-6d65-4f34-9f5c-2931d6d98115", "4aa6710e-68a7-4420-8f7e-cf59fd0f9b13", "Admin", "ADMIN" },
                    { "a46b133f-912a-4e09-a0dc-e215803b23b6", "3f03dadf-64b7-4a16-834c-1573b4102735", "Buyer", "BUYER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");
        }
    }
}
