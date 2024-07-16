using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Solar1TesterSorterLableGenerator.Migrations
{
    public partial class addPacking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bs_NewTesterSorterPacking",
                columns: table => new
                {
                    NewTesterSorterPackingID = table.Column<Guid>(nullable: false),
                    BinNo = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true),
                    Eff = table.Column<string>(nullable: true),
                    Power = table.Column<string>(nullable: true),
                    Lot = table.Column<string>(nullable: true),
                    Shift = table.Column<string>(nullable: true),
                    BatchNumber = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    PDate = table.Column<string>(nullable: true),
                    PackSerialNumber = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SystemUserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bs_NewTesterSorterPacking", x => x.NewTesterSorterPackingID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bs_NewTesterSorterPacking");
        }
    }
}
