using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLog.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddMovementDtosView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               CREATE VIEW MovementDtos AS
                   SELECT Movements.Id
                  ,CargoNr
                  ,adrDelivery.Name + char(13) + adrDelivery.PostCode + ' ' + adrDelivery.City AS Delivery
                  ,PickupId
                  ,CargoPayerId
	              ,UserName
                FROM Movements
                LEFT OUTER JOIN Addresses adrDelivery ON adrDelivery.ID = DeliveryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW MovementDtos;");
        }
    }
}
