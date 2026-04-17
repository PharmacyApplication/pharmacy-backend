using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PharmacyAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "CreatedAt", "Email", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 1, "Pharmacy HQ", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@pharmacy.com", "Admin", true, "$2a$11$OMbcoBQgvgY51jpeyCwfx.jyG.u0AN.vTQwpdpXmDLJWN8zUhvT.q", "9999999999", "Admin" },
                    { 2, "Pharmacy Admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@pharmacy.com", "Admin", true, "$2y$11$vpr.RUTqVQuvEZZre4qn6u1mHDJTvv8HgdB61YJ6joELxzl4L98Vy", "8888888888", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);
        }
    }
}
