using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ProductStatusHistories",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Products",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsUsed",
                table: "PasswordResetRequests",
                newName: "Status");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "SuperCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "ProductStocks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Colors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "SuperCategories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProductStocks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ProductStatusHistories",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Products",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "PasswordResetRequests",
                newName: "IsUsed");
        }
    }
}
