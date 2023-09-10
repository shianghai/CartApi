using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CartApi.Migrations
{
    public partial class AddedUniqueIdFiedToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                table: "Item");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "543c39b8-9afe-4dbd-b8ec-e019a26331d1");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "eed2a54c-4d91-4642-b7ab-5f500e7742d3");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Item",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Item",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                table: "Item",
                column: "Id");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "728d409b-be92-46c7-8110-b5d68c75bfce", "53615e74-0d07-441f-bb0c-28780f19fb5d", "user", "USER" },
                    { "fd49eeeb-7221-4c84-865c-01506c5b57cd", "e82a433b-e21e-4492-bbcf-d9a8adef2f19", "admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Item",
                table: "Item");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "728d409b-be92-46c7-8110-b5d68c75bfce");

            migrationBuilder.DeleteData(
                table: "IdentityRole",
                keyColumn: "Id",
                keyValue: "fd49eeeb-7221-4c84-865c-01506c5b57cd");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Item");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Item",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Item",
                table: "Item",
                column: "ItemId");

            migrationBuilder.InsertData(
                table: "IdentityRole",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "eed2a54c-4d91-4642-b7ab-5f500e7742d3", "7f971c93-de59-4afa-8930-a81c53ab8d94", "user", "USER" },
                    { "543c39b8-9afe-4dbd-b8ec-e019a26331d1", "4e53d268-2c32-4420-ac97-8d64c9de57a5", "admin", "ADMIN" }
                });
        }
    }
}
