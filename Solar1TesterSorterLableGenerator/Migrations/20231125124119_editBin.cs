using Microsoft.EntityFrameworkCore.Migrations;

namespace Solar1TesterSorterLableGenerator.Migrations
{
    public partial class editBin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPower",
                table: "Bs_BinRecipeModel");

            migrationBuilder.DropColumn(
                name: "MinPower",
                table: "Bs_BinRecipeModel");

            migrationBuilder.AddColumn<string>(
                name: "Eff",
                table: "Bs_BinRecipeModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Power",
                table: "Bs_BinRecipeModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eff",
                table: "Bs_BinRecipeModel");

            migrationBuilder.DropColumn(
                name: "Power",
                table: "Bs_BinRecipeModel");

            migrationBuilder.AddColumn<string>(
                name: "MaxPower",
                table: "Bs_BinRecipeModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinPower",
                table: "Bs_BinRecipeModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
