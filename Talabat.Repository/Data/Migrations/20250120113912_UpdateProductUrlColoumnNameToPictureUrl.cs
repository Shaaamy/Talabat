using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Data.migrations
{
    /// <inheritdoc />
    public partial class UpdateProductUrlColoumnNameToPictureUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product_ProductUrl",
                table: "OrderItems",
                newName: "Product_PictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Product_PictureUrl",
                table: "OrderItems",
                newName: "Product_ProductUrl");
        }
    }
}
