using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketApp.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urunler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrunAd = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BirimFiyat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Resim = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urunler", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Urunler",
                columns: new[] { "Id", "BirimFiyat", "Resim", "UrunAd" },
                values: new object[] { 1, 7.90m, "domates.jpg", "Domates" });

            migrationBuilder.InsertData(
                table: "Urunler",
                columns: new[] { "Id", "BirimFiyat", "Resim", "UrunAd" },
                values: new object[] { 2, 12.90m, "patlican.jpg", "Patlıcan" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Urunler");
        }
    }
}
