using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyLog.Data.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class _20250116161257_AutoMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Movements",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Movement_UserName",
                table: "Movements",
                column: "UserName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movement_UserName",
                table: "Movements");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Movements",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
