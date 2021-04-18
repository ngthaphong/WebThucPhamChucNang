using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace WebApplication3.Helper
{
    public static class RegexUtils
    {
        public static readonly Regex SlugRegex = new Regex(@"(^[a-z0-9])([a-z0-9_-]+)*([a-z0-9])$");
    }
    public static class StringExtensions
    {
        //<see cref="System.String.IsNullOrEmpty(string)" />
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !value.IsNullOrEmpty();
        }

        public static string ToSlug(this string value, int? maxLength = null)
        {
            if (RegexUtils.SlugRegex.IsMatch(value))
                return value;
            return GenerateSlug(value, maxLength);
        }

        private static string GenerateSlug(string value, int? maxLength = null)
        {
            var result = RemoveAccent(value).Replace("-", "").ToLowerInvariant();
            result = Regex.Replace(result, @"[^a-z0-9\s-]", string.Empty);
            result = Regex.Replace(result, @"\s+", " ").Trim();
            if (maxLength.HasValue)
                result = result.Substring(0, result.Length <= maxLength ? result.Length : maxLength.Value).Trim();
            return Regex.Replace(result, @"\s", "-");
        }

        private static string RemoveAccent(string value)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

    }
}