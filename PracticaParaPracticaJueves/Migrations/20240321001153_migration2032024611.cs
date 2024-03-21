using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticaParaPracticaJueves.Migrations
{
    public partial class migration2032024611 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Rols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rols", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RolId",
                table: "Users",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Rols_RolId",
                table: "Users",
                column: "RolId",
                principalTable: "Rols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Rols_RolId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Rols");

            migrationBuilder.DropIndex(
                name: "IX_Users_RolId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
