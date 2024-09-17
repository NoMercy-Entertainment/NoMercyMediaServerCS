using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoMercy.Database.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserData_VideoFileId",
                table: "UserData",
                column: "VideoFileId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityProvider_Country",
                table: "PriorityProvider",
                column: "Country");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityProvider_Priority",
                table: "PriorityProvider",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTrack_PlaylistId",
                table: "PlaylistTrack",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationUser_NotificationId",
                table: "NotificationUser",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicGenreTrack_GenreId",
                table: "MusicGenreTrack",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MusicGenreReleaseGroup_GenreId",
                table: "MusicGenreReleaseGroup",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieUser_MovieId",
                table: "MovieUser",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryUser_LibraryId",
                table: "LibraryUser",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTv_LibraryId",
                table: "LibraryTv",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryTrack_LibraryId",
                table: "LibraryTrack",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LibraryMovie_LibraryId",
                table: "LibraryMovie",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageLibrary_LanguageId",
                table: "LanguageLibrary",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordTv_KeywordId",
                table: "KeywordTv",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordMovie_KeywordId",
                table: "KeywordMovie",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CastCreditId",
                table: "Images",
                column: "CastCreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CrewCreditId",
                table: "Images",
                column: "CrewCreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FilePath",
                table: "Images",
                column: "FilePath");

            migrationBuilder.CreateIndex(
                name: "IX_GuestStars_CreditId",
                table: "GuestStars",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreTv_GenreId",
                table: "GenreTv",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_GenreId",
                table: "GenreMovie",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_FolderLibrary_FolderId",
                table: "FolderLibrary",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMovie_FileId",
                table: "FileMovie",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLibrary_FileId",
                table: "FileLibrary",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_EncoderProfileFolder_EncoderProfileId",
                table: "EncoderProfileFolder",
                column: "EncoderProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_CreditId",
                table: "Crews",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionUser_CollectionId",
                table: "CollectionUser",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionMovie_CollectionId",
                table: "CollectionMovie",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionLibrary_CollectionId",
                table: "CollectionLibrary",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificationTv_CertificationId",
                table: "CertificationTv",
                column: "CertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificationMovie_CertificationId",
                table: "CertificationMovie",
                column: "CertificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistUser_ArtistId",
                table: "ArtistUser",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTrack_ArtistId",
                table: "ArtistTrack",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistReleaseGroup_ArtistId",
                table: "ArtistReleaseGroup",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistMusicGenre_ArtistId",
                table: "ArtistMusicGenre",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ArtistLibrary_ArtistId",
                table: "ArtistLibrary",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumUser_AlbumId",
                table: "AlbumUser",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumTrack_AlbumId",
                table: "AlbumTrack",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumReleaseGroup_AlbumId",
                table: "AlbumReleaseGroup",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumMusicGenre_AlbumId",
                table: "AlbumMusicGenre",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumLibrary_AlbumId",
                table: "AlbumLibrary",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumArtist_AlbumId",
                table: "AlbumArtist",
                column: "AlbumId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserData_VideoFileId",
                table: "UserData");

            migrationBuilder.DropIndex(
                name: "IX_PriorityProvider_Country",
                table: "PriorityProvider");

            migrationBuilder.DropIndex(
                name: "IX_PriorityProvider_Priority",
                table: "PriorityProvider");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistTrack_PlaylistId",
                table: "PlaylistTrack");

            migrationBuilder.DropIndex(
                name: "IX_NotificationUser_NotificationId",
                table: "NotificationUser");

            migrationBuilder.DropIndex(
                name: "IX_MusicGenreTrack_GenreId",
                table: "MusicGenreTrack");

            migrationBuilder.DropIndex(
                name: "IX_MusicGenreReleaseGroup_GenreId",
                table: "MusicGenreReleaseGroup");

            migrationBuilder.DropIndex(
                name: "IX_MovieUser_MovieId",
                table: "MovieUser");

            migrationBuilder.DropIndex(
                name: "IX_LibraryUser_LibraryId",
                table: "LibraryUser");

            migrationBuilder.DropIndex(
                name: "IX_LibraryTv_LibraryId",
                table: "LibraryTv");

            migrationBuilder.DropIndex(
                name: "IX_LibraryTrack_LibraryId",
                table: "LibraryTrack");

            migrationBuilder.DropIndex(
                name: "IX_LibraryMovie_LibraryId",
                table: "LibraryMovie");

            migrationBuilder.DropIndex(
                name: "IX_LanguageLibrary_LanguageId",
                table: "LanguageLibrary");

            migrationBuilder.DropIndex(
                name: "IX_KeywordTv_KeywordId",
                table: "KeywordTv");

            migrationBuilder.DropIndex(
                name: "IX_KeywordMovie_KeywordId",
                table: "KeywordMovie");

            migrationBuilder.DropIndex(
                name: "IX_Images_CastCreditId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CrewCreditId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_FilePath",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_GuestStars_CreditId",
                table: "GuestStars");

            migrationBuilder.DropIndex(
                name: "IX_GenreTv_GenreId",
                table: "GenreTv");

            migrationBuilder.DropIndex(
                name: "IX_GenreMovie_GenreId",
                table: "GenreMovie");

            migrationBuilder.DropIndex(
                name: "IX_FolderLibrary_FolderId",
                table: "FolderLibrary");

            migrationBuilder.DropIndex(
                name: "IX_FileMovie_FileId",
                table: "FileMovie");

            migrationBuilder.DropIndex(
                name: "IX_FileLibrary_FileId",
                table: "FileLibrary");

            migrationBuilder.DropIndex(
                name: "IX_EncoderProfileFolder_EncoderProfileId",
                table: "EncoderProfileFolder");

            migrationBuilder.DropIndex(
                name: "IX_Crews_CreditId",
                table: "Crews");

            migrationBuilder.DropIndex(
                name: "IX_CollectionUser_CollectionId",
                table: "CollectionUser");

            migrationBuilder.DropIndex(
                name: "IX_CollectionMovie_CollectionId",
                table: "CollectionMovie");

            migrationBuilder.DropIndex(
                name: "IX_CollectionLibrary_CollectionId",
                table: "CollectionLibrary");

            migrationBuilder.DropIndex(
                name: "IX_CertificationTv_CertificationId",
                table: "CertificationTv");

            migrationBuilder.DropIndex(
                name: "IX_CertificationMovie_CertificationId",
                table: "CertificationMovie");

            migrationBuilder.DropIndex(
                name: "IX_ArtistUser_ArtistId",
                table: "ArtistUser");

            migrationBuilder.DropIndex(
                name: "IX_ArtistTrack_ArtistId",
                table: "ArtistTrack");

            migrationBuilder.DropIndex(
                name: "IX_ArtistReleaseGroup_ArtistId",
                table: "ArtistReleaseGroup");

            migrationBuilder.DropIndex(
                name: "IX_ArtistMusicGenre_ArtistId",
                table: "ArtistMusicGenre");

            migrationBuilder.DropIndex(
                name: "IX_ArtistLibrary_ArtistId",
                table: "ArtistLibrary");

            migrationBuilder.DropIndex(
                name: "IX_AlbumUser_AlbumId",
                table: "AlbumUser");

            migrationBuilder.DropIndex(
                name: "IX_AlbumTrack_AlbumId",
                table: "AlbumTrack");

            migrationBuilder.DropIndex(
                name: "IX_AlbumReleaseGroup_AlbumId",
                table: "AlbumReleaseGroup");

            migrationBuilder.DropIndex(
                name: "IX_AlbumMusicGenre_AlbumId",
                table: "AlbumMusicGenre");

            migrationBuilder.DropIndex(
                name: "IX_AlbumLibrary_AlbumId",
                table: "AlbumLibrary");

            migrationBuilder.DropIndex(
                name: "IX_AlbumArtist_AlbumId",
                table: "AlbumArtist");
        }
    }
}
