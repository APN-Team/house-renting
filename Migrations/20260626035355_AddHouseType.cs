using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace house_renting.Migrations
{
    /// <inheritdoc />
    public partial class AddHouseType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HouseType",
                table: "Houses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseType",
                table: "Houses");
        }
    }
}
