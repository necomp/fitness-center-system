using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace fitcensys.Migrations
{
    /// <inheritdoc />
    public partial class key_problem_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices",
                column: "TrainerServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerServices_TrainerID",
                table: "TrainerServices",
                column: "TrainerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices");

            migrationBuilder.DropIndex(
                name: "IX_TrainerServices_TrainerID",
                table: "TrainerServices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainerServices",
                table: "TrainerServices",
                columns: new[] { "TrainerID", "ServiceDefinitionID" });
        }
    }
}
