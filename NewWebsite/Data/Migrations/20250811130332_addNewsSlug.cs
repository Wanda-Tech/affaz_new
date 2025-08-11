using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class addNewsSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CssColor",
                table: "NewsCategories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "News",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");


            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.objects WHERE type = 'FN' AND name = 'SlugifyString')
                    BEGIN
                        DROP FUNCTION dbo.SlugifyString
                    END
                    GO

                CREATE FUNCTION dbo.SlugifyString (@input NVARCHAR(MAX))
                RETURNS NVARCHAR(MAX)
                AS
                BEGIN
                    -- Convert to lowercase
                    SET @input = LOWER(@input)
                    
                    -- Replace spaces with hyphens
                    SET @input = REPLACE(@input, ' ', '-')
                    
                    -- Remove special characters (keeps a-z, 0-9, and hyphen)
                    DECLARE @result NVARCHAR(MAX) = ''
                    DECLARE @i INT = 1
                    
                    WHILE @i <= LEN(@input)
                    BEGIN
                        DECLARE @char NCHAR(1) = SUBSTRING(@input, @i, 1)
                        
                        IF @char LIKE '[a-z0-9-]'
                            SET @result = @result + @char
                        
                        SET @i = @i + 1
                    END
                    
                    -- Remove consecutive hyphens and trim
                    WHILE CHARINDEX('--', @result) > 0
                        SET @result = REPLACE(@result, '--', '-')
                    
                    SET @result = TRIM('-' FROM @result)
                    
                    RETURN @result
                END;
            ");

            migrationBuilder.Sql(@"
                UPDATE News SET Slug = dbo.SlugifyString(Title);");


            migrationBuilder.Sql(@"
                UPDATE NewsCategories SET CssColor = 'blue' WHERE Id = 1;
                UPDATE NewsCategories SET CssColor = 'green' WHERE Id = 2;
                UPDATE NewsCategories SET CssColor = 'red' WHERE Id = 3;
                UPDATE NewsCategories SET CssColor = 'yellow' WHERE Id = 4;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CssColor",
                table: "NewsCategories");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "News");
        }
    }
}
