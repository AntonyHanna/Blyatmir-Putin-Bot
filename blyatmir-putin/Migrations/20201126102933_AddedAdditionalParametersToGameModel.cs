using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace blyatmir_putin.Migrations
{
    public partial class AddedAdditionalParametersToGameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EffectiveDate",
                table: "Games",
                newName: "StartDate");

            migrationBuilder.AddColumn<string>(
                name: "BannerUri",
                table: "Games",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PosterUri",
                table: "Games",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerUri",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PosterUri",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Games",
                newName: "EffectiveDate");
        }
    }
}
