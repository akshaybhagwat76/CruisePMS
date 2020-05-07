using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CruisePMS.Migrations
{
    public partial class addedmissingtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MasterAmenities",
                table: "MasterAmenities");

            migrationBuilder.RenameTable(
                name: "MasterAmenities",
                newName: "AppMasterAmenities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppMasterAmenities",
                table: "AppMasterAmenities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppCruiseItineraries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ItineraryName = table.Column<string>(nullable: true),
                    ItineraryCode = table.Column<string>(nullable: true),
                    ItineraryMap = table.Column<byte[]>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Lang = table.Column<string>(nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OnBoardService = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruiseItineraries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCruiseServiceGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    IsMainService = table.Column<bool>(nullable: false),
                    OnlyOneCanBeChoosen = table.Column<bool>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    StopDate = table.Column<DateTime>(nullable: true),
                    ServiceGroupName = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruiseServiceGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCruiseServiceGroup_AppMasterAmenities_ServiceGroupName",
                        column: x => x.ServiceGroupName,
                        principalTable: "AppMasterAmenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppCruiseServiceUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ServiceUnit = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruiseServiceUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCruiseServiceUnit_AppMasterAmenities_ServiceUnit",
                        column: x => x.ServiceUnit,
                        principalTable: "AppMasterAmenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppCruiseShipCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    CruiseShipCategoryName = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruiseShipCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCruiseThemes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CruiseThemeDescription = table.Column<string>(nullable: true),
                    CruiseThemeName = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruiseThemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCruiseThemes_AppMasterAmenities_CruiseThemeName",
                        column: x => x.CruiseThemeName,
                        principalTable: "AppMasterAmenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppTenantTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantTypeName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTenantTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCruiseService",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    CruiseServiceGroupsId = table.Column<int>(nullable: true),
                    CruiseServiceUnitsId = table.Column<int>(nullable: true),
                    ServiceName = table.Column<int>(nullable: true),
                    PayOnSpot = table.Column<bool>(nullable: false),
                    ReductionCanBeApplied = table.Column<bool>(nullable: false),
                    Obligatory = table.Column<bool>(nullable: false),
                    TenantRecipient = table.Column<int>(nullable: true),
                    Lang = table.Column<string>(nullable: true),
                    Taxable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruiseService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCruiseService_AppCruiseServiceGroup_CruiseServiceGroupsId",
                        column: x => x.CruiseServiceGroupsId,
                        principalTable: "AppCruiseServiceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppCruiseService_AppCruiseServiceUnit_CruiseServiceUnitsId",
                        column: x => x.CruiseServiceUnitsId,
                        principalTable: "AppCruiseServiceUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppCruiseService_AppMasterAmenities_ServiceName",
                        column: x => x.ServiceName,
                        principalTable: "AppMasterAmenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppCruiseShip",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    CruiseShipName = table.Column<string>(nullable: false),
                    CruiseDecksNumber = table.Column<int>(nullable: false),
                    CruiseShipBuided = table.Column<int>(nullable: true),
                    CruiseShipFlag = table.Column<string>(nullable: true),
                    CruiseShipHomePort = table.Column<string>(nullable: true),
                    CruiseShipRefurbished = table.Column<int>(nullable: true),
                    CruiseShipLength = table.Column<int>(nullable: true),
                    CruiseShipDraft = table.Column<int>(nullable: true),
                    CruiseShipMaxSpeed = table.Column<int>(nullable: true),
                    CruiseShipSpeed = table.Column<int>(nullable: true),
                    CruiseShipWidth = table.Column<int>(nullable: true),
                    CruiseShipCabinsNumber = table.Column<int>(nullable: true),
                    CruiseShipCrewNumber = table.Column<int>(nullable: true),
                    CruiseShipPassangersNumber = table.Column<int>(nullable: true),
                    CruiseShipVoltage = table.Column<string>(nullable: true),
                    CruiseShipIsEnabled = table.Column<bool>(nullable: false),
                    CruiseShipShortDescription = table.Column<string>(nullable: true),
                    CruiseShipDescription = table.Column<string>(nullable: true),
                    Lang = table.Column<string>(nullable: true),
                    CruiseShipCategoryId = table.Column<int>(nullable: true),
                    OwnerTenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruiseShip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCruiseShip_AppCruiseShipCategory_CruiseShipCategoryId",
                        column: x => x.CruiseShipCategoryId,
                        principalTable: "AppCruiseShipCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppTenantTypesPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    TenantRecipient = table.Column<string>(nullable: true),
                    EntityName = table.Column<string>(nullable: false),
                    TenantTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTenantTypesPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTenantTypesPermissions_AppTenantTypes_TenantTypeID",
                        column: x => x.TenantTypeID,
                        principalTable: "AppTenantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppCruises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    CruiseDuration = table.Column<int>(nullable: false),
                    CruiseStartPort = table.Column<int>(nullable: false),
                    CruiseEndPort = table.Column<int>(nullable: false),
                    Cruise_Airport = table.Column<string>(nullable: false),
                    CruiseIsEnabled = table.Column<bool>(nullable: false),
                    CruiseIsEnabledB2B = table.Column<bool>(nullable: false),
                    DisableForApi = table.Column<bool>(nullable: false),
                    StandardDeposit = table.Column<decimal>(nullable: false),
                    DepositType = table.Column<int>(nullable: false),
                    CheckIn = table.Column<DateTime>(nullable: false),
                    Checkout = table.Column<DateTime>(nullable: false),
                    CruiseShipsId = table.Column<int>(nullable: true),
                    CruiseThemesId = table.Column<int>(nullable: true),
                    CruiseServicesId = table.Column<int>(nullable: true),
                    CruiseItinerariesId = table.Column<int>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    VirtualCruise = table.Column<bool>(nullable: false),
                    CruiseYear = table.Column<int>(nullable: false),
                    FreeInternet = table.Column<bool>(nullable: false),
                    TransferIncluded = table.Column<bool>(nullable: false),
                    CruiseOperatorId = table.Column<int>(nullable: true),
                    BookingEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCruises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCruises_AppCruiseItineraries_CruiseItinerariesId",
                        column: x => x.CruiseItinerariesId,
                        principalTable: "AppCruiseItineraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppCruises_AppCruiseService_CruiseServicesId",
                        column: x => x.CruiseServicesId,
                        principalTable: "AppCruiseService",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppCruises_AppCruiseShip_CruiseShipsId",
                        column: x => x.CruiseShipsId,
                        principalTable: "AppCruiseShip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppCruises_AppCruiseThemes_CruiseThemesId",
                        column: x => x.CruiseThemesId,
                        principalTable: "AppCruiseThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCruises_CruiseItinerariesId",
                table: "AppCruises",
                column: "CruiseItinerariesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruises_CruiseServicesId",
                table: "AppCruises",
                column: "CruiseServicesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruises_CruiseShipsId",
                table: "AppCruises",
                column: "CruiseShipsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruises_CruiseThemesId",
                table: "AppCruises",
                column: "CruiseThemesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruiseService_CruiseServiceGroupsId",
                table: "AppCruiseService",
                column: "CruiseServiceGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruiseService_CruiseServiceUnitsId",
                table: "AppCruiseService",
                column: "CruiseServiceUnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruiseService_ServiceName",
                table: "AppCruiseService",
                column: "ServiceName");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruiseServiceGroup_ServiceGroupName",
                table: "AppCruiseServiceGroup",
                column: "ServiceGroupName");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruiseServiceUnit_ServiceUnit",
                table: "AppCruiseServiceUnit",
                column: "ServiceUnit");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruiseShip_CruiseShipCategoryId",
                table: "AppCruiseShip",
                column: "CruiseShipCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCruiseThemes_CruiseThemeName",
                table: "AppCruiseThemes",
                column: "CruiseThemeName");

            migrationBuilder.CreateIndex(
                name: "IX_AppTenantTypesPermissions_TenantTypeID",
                table: "AppTenantTypesPermissions",
                column: "TenantTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppCruises");

            migrationBuilder.DropTable(
                name: "AppTenantTypesPermissions");

            migrationBuilder.DropTable(
                name: "AppCruiseItineraries");

            migrationBuilder.DropTable(
                name: "AppCruiseService");

            migrationBuilder.DropTable(
                name: "AppCruiseShip");

            migrationBuilder.DropTable(
                name: "AppCruiseThemes");

            migrationBuilder.DropTable(
                name: "AppTenantTypes");

            migrationBuilder.DropTable(
                name: "AppCruiseServiceGroup");

            migrationBuilder.DropTable(
                name: "AppCruiseServiceUnit");

            migrationBuilder.DropTable(
                name: "AppCruiseShipCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppMasterAmenities",
                table: "AppMasterAmenities");

            migrationBuilder.RenameTable(
                name: "AppMasterAmenities",
                newName: "MasterAmenities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MasterAmenities",
                table: "MasterAmenities",
                column: "Id");
        }
    }
}
