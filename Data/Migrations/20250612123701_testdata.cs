using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class testdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_carListings_CarId",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_customers_CustomerId",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_carListings",
                table: "carListings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "carListings",
                newName: "CarListings");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_CustomerId",
                table: "Bookings",
                newName: "IX_Bookings_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_CarId",
                table: "Bookings",
                newName: "IX_Bookings_CarId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarListings",
                table: "CarListings",
                column: "CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "BookingId");

            migrationBuilder.CreateTable(
                name: "CustomerViewModel",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    CustomerId1 = table.Column<int>(type: "int", nullable: false),
                    CustomerViewModelCustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerViewModel", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_CustomerViewModel_CustomerViewModel_CustomerViewModelCustomerId",
                        column: x => x.CustomerViewModelCustomerId,
                        principalTable: "CustomerViewModel",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_CustomerViewModel_Customers_CustomerId1",
                        column: x => x.CustomerId1,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerViewModel_CustomerId1",
                table: "CustomerViewModel",
                column: "CustomerId1");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerViewModel_CustomerViewModelCustomerId",
                table: "CustomerViewModel",
                column: "CustomerViewModelCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CarListings_CarId",
                table: "Bookings",
                column: "CarId",
                principalTable: "CarListings",
                principalColumn: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Customers_CustomerId",
                table: "Bookings",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CarListings_CarId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Customers_CustomerId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "CustomerViewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarListings",
                table: "CarListings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "customers");

            migrationBuilder.RenameTable(
                name: "CarListings",
                newName: "carListings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CustomerId",
                table: "bookings",
                newName: "IX_bookings_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CarId",
                table: "bookings",
                newName: "IX_bookings_CarId");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_carListings",
                table: "carListings",
                column: "CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_carListings_CarId",
                table: "bookings",
                column: "CarId",
                principalTable: "carListings",
                principalColumn: "CarId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_customers_CustomerId",
                table: "bookings",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
