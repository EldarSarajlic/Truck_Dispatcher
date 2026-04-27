using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dispatcher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryPhotoFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Inventory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_PhotoId",
                table: "Inventory",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Photos_PhotoId",
                table: "Inventory",
                column: "PhotoId",
                principalTable: "Photos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Photos_PhotoId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_PhotoId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Inventory");
        }
    }
}
