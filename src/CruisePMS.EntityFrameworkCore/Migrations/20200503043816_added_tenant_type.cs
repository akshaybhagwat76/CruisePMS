using Microsoft.EntityFrameworkCore.Migrations;

namespace CruisePMS.Migrations
{
    public partial class added_tenant_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantType",
                table: "AbpTenants",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantType",
                table: "AbpTenants");
        }
    }
}
