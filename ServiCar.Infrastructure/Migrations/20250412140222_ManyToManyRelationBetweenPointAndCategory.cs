using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiCar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyRelationBetweenPointAndCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_Categories_CategoryId",
                table: "Points");

            migrationBuilder.DropIndex(
                name: "IX_Points_CategoryId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Points");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "Images",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "CategoryPoint",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    PointsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPoint", x => new { x.CategoriesId, x.PointsId });
                    table.ForeignKey(
                        name: "FK_CategoryPoint_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryPoint_Points_PointsId",
                        column: x => x.PointsId,
                        principalTable: "Points",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPoint_PointsId",
                table: "CategoryPoint",
                column: "PointsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryPoint");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Points_CategoryId",
                table: "Points",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_Categories_CategoryId",
                table: "Points",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
