using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedSysProject.Migrations
{
    /// <inheritdoc />
    public partial class AddMemberImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "BlogCategory");
        }
    }
}
