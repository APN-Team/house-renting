using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace house_renting.Migrations
{
    /// <inheritdoc />
    public partial class AddPreferredPaymentPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredPaymentPlan",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "Monthly");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredPaymentPlan",
                table: "AspNetUsers");
        }
    }
}
