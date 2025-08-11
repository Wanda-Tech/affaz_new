using System.Text.RegularExpressions;

namespace NewWebsite.Extension;

public static class StringExtension
{
    public static string Slugify(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Convert to lowercase
        string slug = input.ToLowerInvariant();

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s", "-", RegexOptions.Compiled);

        // Remove all non-alphanumeric characters except hyphens
        slug = Regex.Replace(slug, @"[^a-z0-9-]", "", RegexOptions.Compiled);

        // Replace multiple hyphens with single hyphen
        slug = Regex.Replace(slug, @"-+", "-", RegexOptions.Compiled);

        // Trim hyphens from start and end
        slug = slug.Trim('-');

        return slug;
    }
}