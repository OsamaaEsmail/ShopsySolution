using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditUserNameAndCatalogPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "ClaimValue",
                value: "categories:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "ClaimValue",
                value: "categories:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "ClaimValue",
                value: "categories:update");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "ClaimValue",
                value: "categories:delete");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "ClaimValue",
                value: "subcategories:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "ClaimValue",
                value: "subcategories:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "ClaimValue",
                value: "subcategories:update");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "ClaimValue",
                value: "subcategories:delete");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "ClaimValue",
                value: "vendors:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14,
                column: "ClaimValue",
                value: "vendors:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15,
                column: "ClaimValue",
                value: "vendors:update");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16,
                column: "ClaimValue",
                value: "vendors:delete");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17,
                column: "ClaimValue",
                value: "stocks:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18,
                column: "ClaimValue",
                value: "stocks:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19,
                column: "ClaimValue",
                value: "stocks:update");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20,
                column: "ClaimValue",
                value: "orders:read");

            migrationBuilder.InsertData(
                schema: "users",
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 21, "permissions", "orders:add", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 22, "permissions", "orders:update", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 23, "permissions", "orders:delete", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 24, "permissions", "sales:read", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 25, "permissions", "sales:add", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 26, "permissions", "sales:update", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 27, "permissions", "sales:delete", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 28, "permissions", "carts:read", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 29, "permissions", "carts:manage", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 30, "permissions", "users:read", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 31, "permissions", "users:add", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 32, "permissions", "users:update", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 33, "permissions", "roles:read", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 34, "permissions", "roles:add", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" },
                    { 35, "permissions", "roles:update", "7e6afdc1-9582-4330-a0cd-8b41490a39b7" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "ClaimValue",
                value: "orders:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "ClaimValue",
                value: "orders:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "ClaimValue",
                value: "orders:update");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "ClaimValue",
                value: "orders:delete");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "ClaimValue",
                value: "sales:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10,
                column: "ClaimValue",
                value: "sales:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11,
                column: "ClaimValue",
                value: "sales:update");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12,
                column: "ClaimValue",
                value: "sales:delete");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13,
                column: "ClaimValue",
                value: "carts:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14,
                column: "ClaimValue",
                value: "carts:manage");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15,
                column: "ClaimValue",
                value: "users:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16,
                column: "ClaimValue",
                value: "users:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17,
                column: "ClaimValue",
                value: "users:update");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18,
                column: "ClaimValue",
                value: "roles:read");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19,
                column: "ClaimValue",
                value: "roles:add");

            migrationBuilder.UpdateData(
                schema: "users",
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20,
                column: "ClaimValue",
                value: "roles:update");
        }
    }
}
