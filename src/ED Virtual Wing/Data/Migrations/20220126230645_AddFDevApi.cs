using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED_Virtual_Wing.Data.Migrations
{
    public partial class AddFDevApi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FDevCustomerId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "FDevApiAuthCode",
                columns: table => new
                {
                    State = table.Column<string>(type: "varchar(128)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Code = table.Column<string>(type: "varchar(128)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Created = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FDevApiAuthCode", x => x.State);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FDevCustomerId",
                table: "AspNetUsers",
                column: "FDevCustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FDevApiAuthCode");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FDevCustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FDevCustomerId",
                table: "AspNetUsers");
        }
    }
}
