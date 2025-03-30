using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiCar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnBusinessStatusId_And_MakeImageIdNullable_BusinessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Businesses_ImageId",
                table: "Businesses");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Businesses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BusinessStatusId",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_ImageId",
                table: "Businesses",
                column: "ImageId",
                unique: true,
                filter: "[ImageId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Businesses_ImageId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "BusinessStatusId",
                table: "Businesses");

            migrationBuilder.AlterColumn<int>(
                name: "ImageId",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_ImageId",
                table: "Businesses",
                column: "ImageId",
                unique: true);
        }
    }
}
