using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class new0612241541 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSize",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "iller");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ilceler");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductStocks",
                newName: "ProductDetailsId");

            migrationBuilder.RenameColumn(
                name: "ProductStockId",
                table: "ProductStocks",
                newName: "ProductStocksId");

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    ProductDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.ProductDetailsId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.RenameColumn(
                name: "ProductDetailsId",
                table: "ProductStocks",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductStocksId",
                table: "ProductStocks",
                newName: "ProductStockId");

            migrationBuilder.AddColumn<string>(
                name: "ProductSize",
                table: "ProductStocks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "iller",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ilceler",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
