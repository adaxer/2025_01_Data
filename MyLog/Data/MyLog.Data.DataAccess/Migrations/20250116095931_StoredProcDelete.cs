using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLog.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE DeleteMovementById
                    @Id INT
                AS
                BEGIN
                    DELETE FROM Movements WHERE Id = @Id
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE DeleteMovementById;");
        }
    }
}
