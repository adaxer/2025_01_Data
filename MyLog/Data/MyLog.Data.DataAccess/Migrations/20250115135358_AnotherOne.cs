using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLog.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AnotherOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Addresses_CargoPayerId",
                table: "Movements");

            migrationBuilder.AlterColumn<int>(
                name: "CargoPayerId",
                table: "Movements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Addresses_CargoPayerId",
                table: "Movements",
                column: "CargoPayerId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movements_Addresses_CargoPayerId",
                table: "Movements");

            migrationBuilder.AlterColumn<int>(
                name: "CargoPayerId",
                table: "Movements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movements_Addresses_CargoPayerId",
                table: "Movements",
                column: "CargoPayerId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
