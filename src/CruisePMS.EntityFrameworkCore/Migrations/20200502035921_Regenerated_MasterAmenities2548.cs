using Microsoft.EntityFrameworkCore.Migrations;

namespace CruisePMS.Migrations
{
    public partial class Regenerated_MasterAmenities2548 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MasterAmenities_TenantId",
                table: "MasterAmenities");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "MasterAmenities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "MasterAmenities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterAmenities_TenantId",
                table: "MasterAmenities",
                column: "TenantId");
        }
    }
}
