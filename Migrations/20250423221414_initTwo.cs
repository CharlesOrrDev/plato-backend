using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace plato_backend.Migrations
{
    /// <inheritdoc />
    public partial class initTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Friends",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomingFriendRequest",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutgoingFriendRequest",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Friends",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IncomingFriendRequest",
                table: "User");

            migrationBuilder.DropColumn(
                name: "OutgoingFriendRequest",
                table: "User");
        }
    }
}
