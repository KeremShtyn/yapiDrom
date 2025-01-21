using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ScooterId",
                table: "Reservations",
                column: "ScooterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Scooters_ScooterId",
                table: "Reservations",
                column: "ScooterId",
                principalTable: "Scooters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Scooters_ScooterId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ScooterId",
                table: "Reservations");
        }
    }
}
