using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Test.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnedTestEntity_Value1",
                table: "Models",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnedTestEntity_Value2",
                table: "Models",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnedTestEntity_Value1",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "OwnedTestEntity_Value2",
                table: "Models");
        }
    }
}
