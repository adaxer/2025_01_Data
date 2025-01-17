using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLog.Data.DataAccess.Migrations;

/// <inheritdoc />
public partial class Start : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Addresses",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PostCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Addresses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Movements",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CargoNr = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                DeliveryId = table.Column<int>(type: "int", nullable: true),
                PickUpId = table.Column<int>(type: "int", nullable: true),
                CargoPayerId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Movements", x => x.Id);
                table.ForeignKey(
                    name: "FK_Movements_Addresses_CargoPayerId",
                    column: x => x.CargoPayerId,
                    principalTable: "Addresses",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Movements_Addresses_DeliveryId",
                    column: x => x.DeliveryId,
                    principalTable: "Addresses",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Movements_Addresses_PickUpId",
                    column: x => x.PickUpId,
                    principalTable: "Addresses",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_Movements_CargoPayerId",
            table: "Movements",
            column: "CargoPayerId");

        migrationBuilder.CreateIndex(
            name: "IX_Movements_DeliveryId",
            table: "Movements",
            column: "DeliveryId");

        migrationBuilder.CreateIndex(
            name: "IX_Movements_PickUpId",
            table: "Movements",
            column: "PickUpId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Movements");

        migrationBuilder.DropTable(
            name: "Addresses");
    }
}
