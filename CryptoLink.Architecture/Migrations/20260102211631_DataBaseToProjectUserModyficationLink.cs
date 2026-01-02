using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoLink.Architecture.Migrations
{
    /// <inheritdoc />
    public partial class DataBaseToProjectUserModyficationLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LinkExtendeds_LinksExtendedId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LinksExtendedId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LinksExtendedId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "LinkExtendedId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LinkExtendedId",
                table: "Users",
                column: "LinkExtendedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LinkExtendeds_LinkExtendedId",
                table: "Users",
                column: "LinkExtendedId",
                principalTable: "LinkExtendeds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LinkExtendeds_LinkExtendedId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_LinkExtendedId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LinkExtendedId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "LinksExtendedId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LinksExtendedId",
                table: "Users",
                column: "LinksExtendedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LinkExtendeds_LinksExtendedId",
                table: "Users",
                column: "LinksExtendedId",
                principalTable: "LinkExtendeds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
