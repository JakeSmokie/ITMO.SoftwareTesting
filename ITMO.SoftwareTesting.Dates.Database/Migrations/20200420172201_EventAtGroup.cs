using Microsoft.EntityFrameworkCore.Migrations;

namespace ITMO.SoftwareTesting.Dates.Database.Migrations
{
    public partial class EventAtGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventsAtGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsAtGroups", x => new { x.EventId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_EventsAtGroups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventsAtGroups_GroupId",
                table: "EventsAtGroups",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventsAtGroups");
        }
    }
}
