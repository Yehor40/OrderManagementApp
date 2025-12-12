using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class Customerseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusCode",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CustomerName", "Email", "Password" },
                values: new object[,]
                {
                    { 2, "Jane Smith", "jane@example.com", "hashed_password" },
                    { 3, "Acme Corporation LLC", "contact@acme.com", "hashed_password" }
                });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2025, 12, 12, 21, 20, 59, 605, DateTimeKind.Local).AddTicks(870));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusCode",
                table: "Orders",
                column: "StatusCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_StatusCode",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2025, 12, 12, 18, 36, 23, 897, DateTimeKind.Local).AddTicks(9340));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusCode",
                table: "Orders",
                column: "StatusCode",
                unique: true);
        }
    }
}
