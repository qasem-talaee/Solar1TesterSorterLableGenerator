using Microsoft.EntityFrameworkCore.Migrations;

namespace Solar1TesterSorterLableGenerator.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bs_BinRecipeModel",
                columns: table => new
                {
                    BinRecipeModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BinNumber = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    IV = table.Column<string>(nullable: true),
                    MinPower = table.Column<string>(nullable: true),
                    MaxPower = table.Column<string>(nullable: true),
                    Print = table.Column<string>(nullable: true),
                    Class = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bs_BinRecipeModel", x => x.BinRecipeModelID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bs_BinRecipeModel");
        }
    }
}
