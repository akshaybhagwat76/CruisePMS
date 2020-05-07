using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CruisePMS.Migrations
{
    public partial class Added_MasterAmenities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterAmenities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    NewId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Lang = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    DisplayName2 = table.Column<string>(nullable: true),
                    SourceId = table.Column<int>(nullable: true),
                    OrderColumn = table.Column<int>(nullable: true),
                    Original = table.Column<short>(nullable: true),
                    SourceTable = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterAmenities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterAmenities_TenantId",
                table: "MasterAmenities",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterAmenities");
        }
    }
}
