using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Test2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Track",
                table: "VideoFiles",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Configuration",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Configuration",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_TvUser_TvId",
                table: "TvUser",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_AlbumId",
                table: "Translations",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ArtistId",
                table: "Translations",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_CollectionId",
                table: "Translations",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_EpisodeId",
                table: "Translations",
                column: "EpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_MovieId",
                table: "Translations",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_PersonId",
                table: "Translations",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_ReleaseGroupId",
                table: "Translations",
                column: "ReleaseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_SeasonId",
                table: "Translations",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_TvId",
                table: "Translations",
                column: "TvId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUser_TrackId",
                table: "TrackUser",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_Filename",
                table: "Tracks",
                column: "Filename");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Key",
                table: "Configuration",
                column: "Key",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TvUser_TvId",
                table: "TvUser");

            migrationBuilder.DropIndex(
                name: "IX_Translations_AlbumId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ArtistId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_CollectionId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_EpisodeId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_MovieId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_PersonId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_ReleaseGroupId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_SeasonId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_TvId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_TrackUser_TrackId",
                table: "TrackUser");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_Filename",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Configuration_Key",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "Track",
                table: "VideoFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Configuration",
                type: "TEXT",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Configuration",
                type: "TEXT",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);
        }
    }
}
