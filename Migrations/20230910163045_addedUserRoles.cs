using Microsoft.EntityFrameworkCore.Migrations;

namespace CartApi.Migrations
{
    public partial class addedUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "728d409b-be92-46c7-8110-b5d68c75bfce");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "fd49eeeb-7221-4c84-865c-01506c5b57cd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1L, "b899efe3-be76-46f8-8af8-a24684c9c455", "USER", "user" },
                    { 2L, "85272670-d9eb-4f50-89be-95e950dfafaa", "ADMIN", "admin" }
                });

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ecec713c-c336-4a39-af3c-23b1f5b19433", "be192512-68b7-4f96-b527-4738f4200f4a", "user", "USER" },
                    { "01a2bcd9-0c89-446a-a78d-c9e548aa0941", "90e8e70c-9f00-4d0a-b180-f612ad12a06f", "admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "01a2bcd9-0c89-446a-a78d-c9e548aa0941");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "ecec713c-c336-4a39-af3c-23b1f5b19433");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "728d409b-be92-46c7-8110-b5d68c75bfce", "53615e74-0d07-441f-bb0c-28780f19fb5d", "user", "USER" },
                    { "fd49eeeb-7221-4c84-865c-01506c5b57cd", "e82a433b-e21e-4492-bbcf-d9a8adef2f19", "admin", "ADMIN" }
                });
        }
    }
}
