using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RssReader.Migrations;

/// <inheritdoc />
public partial class AddIsRemovedToUserFeedItem : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Username",
            table: "Users",
            newName: "UserName");

        migrationBuilder.AddColumn<bool>(
            name: "IsRemoved",
            table: "UserFeedItems",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsRemoved",
            table: "UserFeedItems");

        migrationBuilder.RenameColumn(
            name: "UserName",
            table: "Users",
            newName: "Username");
    }
}

