using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PracticaParaPracticaJueves.Migrations
{
    public partial class migration4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rols",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "soy admin", "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "RolId", "Username" },
                values: new object[] { 1, "e10adc3949ba59abbe56e057f20f883e", 1, "Root" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rols",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
