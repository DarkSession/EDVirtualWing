using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED_Virtual_Wing.Data.Migrations
{
    public partial class AddShipName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipName",
                table: "Commander",
                type: "varchar(256)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipName",
                table: "Commander");
        }
    }
}
