using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operativka.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsumerCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumerCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrgId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanningIndicators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_Fact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningIndicators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settlements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settlements_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActionsDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SettlementId = table.Column<int>(type: "int", nullable: false),
                    ConsumerCategorieId = table.Column<int>(type: "int", nullable: false),
                    PlanningIndicatorId = table.Column<int>(type: "int", nullable: false),
                    Plan_Count = table.Column<int>(type: "int", nullable: false),
                    Fact_Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionsDocuments_ConsumerCategories_ConsumerCategorieId",
                        column: x => x.ConsumerCategorieId,
                        principalTable: "ConsumerCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionsDocuments_PlanningIndicators_PlanningIndicatorId",
                        column: x => x.PlanningIndicatorId,
                        principalTable: "PlanningIndicators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionsDocuments_Settlements_SettlementId",
                        column: x => x.SettlementId,
                        principalTable: "Settlements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionsDocuments_ConsumerCategorieId",
                table: "ActionsDocuments",
                column: "ConsumerCategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsDocuments_PlanningIndicatorId",
                table: "ActionsDocuments",
                column: "PlanningIndicatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsDocuments_SettlementId",
                table: "ActionsDocuments",
                column: "SettlementId");

            migrationBuilder.CreateIndex(
                name: "IX_Settlements_DistrictId",
                table: "Settlements",
                column: "DistrictId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionsDocuments");

            migrationBuilder.DropTable(
                name: "ConsumerCategories");

            migrationBuilder.DropTable(
                name: "PlanningIndicators");

            migrationBuilder.DropTable(
                name: "Settlements");

            migrationBuilder.DropTable(
                name: "Districts");
        }
    }
}
