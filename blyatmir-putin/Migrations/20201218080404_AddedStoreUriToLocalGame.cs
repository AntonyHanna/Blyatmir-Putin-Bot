using Microsoft.EntityFrameworkCore.Migrations;

namespace blyatmir_putin.Migrations
{
    public partial class AddedStoreUriToLocalGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StorePageUrl",
                table: "Games",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorePageUrl",
                table: "Games");
        }
    }
}
