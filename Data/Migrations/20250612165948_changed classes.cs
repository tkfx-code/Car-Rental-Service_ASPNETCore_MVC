using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class Changedclasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerViewModel_CustomerViewModel_CustomerViewModelCustomerId",
                table: "CustomerViewModel");

            migrationBuilder.DropIndex(
                name: "IX_CustomerViewModel_CustomerViewModelCustomerId",
                table: "CustomerViewModel");

            migrationBuilder.DropColumn(
                name: "CustomerViewModelCustomerId",
                table: "CustomerViewModel");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "TotalPrice",
                table: "CarListings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BookingViewModel",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CarId = table.Column<int>(type: "int", nullable: true),
                    BookingId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingViewModel", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_BookingViewModel_Bookings_BookingId1",
                        column: x => x.BookingId1,
                        principalTable: "Bookings",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingViewModel_CarListings_CarId",
                        column: x => x.CarId,
                        principalTable: "CarListings",
                        principalColumn: "CarId");
                    table.ForeignKey(
                        name: "FK_BookingViewModel_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                });

            migrationBuilder.CreateTable(
                name: "CarListingViewModel",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Make = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<int>(type: "int", nullable: false),
                    isAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CarListingCarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarListingViewModel", x => x.CarId);
                    table.ForeignKey(
                        name: "FK_CarListingViewModel_CarListings_CarListingCarId",
                        column: x => x.CarListingCarId,
                        principalTable: "CarListings",
                        principalColumn: "CarId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingViewModel_BookingId1",
                table: "BookingViewModel",
                column: "BookingId1");

            migrationBuilder.CreateIndex(
                name: "IX_BookingViewModel_CarId",
                table: "BookingViewModel",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingViewModel_CustomerId",
                table: "BookingViewModel",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CarListingViewModel_CarListingCarId",
                table: "CarListingViewModel",
                column: "CarListingCarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingViewModel");

            migrationBuilder.DropTable(
                name: "CarListingViewModel");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "CarListings");

            migrationBuilder.AddColumn<int>(
                name: "CustomerViewModelCustomerId",
                table: "CustomerViewModel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Bookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerViewModel_CustomerViewModelCustomerId",
                table: "CustomerViewModel",
                column: "CustomerViewModelCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerViewModel_CustomerViewModel_CustomerViewModelCustomerId",
                table: "CustomerViewModel",
                column: "CustomerViewModelCustomerId",
                principalTable: "CustomerViewModel",
                principalColumn: "CustomerId");
        }
    }
}
