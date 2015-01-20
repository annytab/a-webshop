using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// This class is a helper for html and string manipulations
    /// Author: Rob Volk, https://github.com/robvolk/
    /// Source: https://github.com/robvolk/Helpers.Net/blob/master/Src/Helpers.Net/StringHtmlExtensions.cs
    /// </summary>
    public static class StringHtmlExtensions
    {
        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        public static string TruncateHtml(this string html, int maxCharacters, string trailingText)
        {
            // Return if the string is null or empty
            if (string.IsNullOrEmpty(html) == true)
            {
                return html;
            }

            // Count the characters and ignore tags
            Int32 textCount = 0;
            Int32 charCount = 0;
            bool ignore = false;

            // Loop all the characters in the html string
            foreach (char c in html)
            {
                // Increase the character count
                charCount++;

                // Count characters that are outside of html tags
                if (c == '<')
                {
                    ignore = true;
                }
                else if (ignore == false)
                {
                    textCount++;
                }

                // Reset the ignore boolean
                if (c == '>')
                {
                    ignore = false;
                }

                // Stop once we hit the limit
                if (textCount >= maxCharacters)
                {
                    break;
                }
            }

            // Truncate the html and keep whole words only
            StringBuilder trunc = new StringBuilder(html.TruncateWords(charCount));

            // Keep track of open tags and close any tags left open
            Stack<string> tags = new Stack<string>();
            MatchCollection matches = Regex.Matches(trunc.ToString(), @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            // Loop all the matches
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string tag = match.Groups["tag"].Value;
                    string closeTag = match.Groups["closeTag"].Value;

                    // Push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                    if (string.IsNullOrEmpty(tag) == false && string.IsNullOrEmpty(match.Groups["selfClose"].Value) == true)
                    {
                        tags.Push(tag);
                    }
                    else if (string.IsNullOrEmpty(closeTag) == false)
                    {
                        // Pop the tag to close it.. find the matching opening tag
                        // ignore any unclosed tags
                        while (tags.Count > 0 && tags.Pop() != closeTag) { }
                    }
                }
            }

            // Add the trailing text
            if (html.Length > charCount)
            {
                trunc.Append(trailingText);
            }

            // Pop the rest off the stack to close remainder of tags
            while (tags.Count > 0)
            {
                trunc.Append("</");
                trunc.Append(tags.Pop());
                trunc.Append('>');
            }

            // Return the html string
            return trunc.ToString();

        } // End of the TruncateHtml method

        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.
        /// </summary>
        public static string TruncateHtml(this string html, int maxCharacters)
        {
            return html.TruncateHtml(maxCharacters, null);

        } // End of the TruncateHtml method

        /// <summary>
        /// Truncates a string containing HTML to the first occurrence of a delimiter
        /// </summary>
        /// <param name="html">The HTML string to truncate</param>
        /// <param name="delimiter">The delimiter</param>
        /// <param name="comparison">The delimiter comparison type</param>
        public static string TruncateHtmlByDelimiter(this string html, string delimiter, StringComparison comparison = StringComparison.Ordinal)
        {
            var index = html.IndexOf(delimiter, comparison);
            if (index <= 0) return html;

            var r = html.Substring(0, index);
            return r.TruncateHtml(r.StripHtml().Length);

        } // End of the TruncateHtmlByDelimiter

        /// <summary>
        /// Strips all HTML tags from a string
        /// </summary>
        public static string StripHtml(this string html)
        {
            if (string.IsNullOrEmpty(html))
                return html;

            return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);

        } // End of the StripHtml method

        /// <summary>
        /// Truncates text to a number of characters
        /// </summary>
        public static string Truncate(this string text, int maxCharacters)
        {
            return text.Truncate(maxCharacters, null);

        } // End of the Truncate method

        /// <summary>
        /// Truncates text to a number of characters and adds trailing text, i.e. elipses, to the end
        /// </summary>
        public static string Truncate(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;
            else
                return text.Substring(0, maxCharacters) + trailingText;

        } // End of the Truncate method

        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        public static string TruncateWords(this string text, int maxCharacters)
        {
            return text.TruncateWords(maxCharacters, null);

        } // End of the TruncateWords method

        /// <summary>
        /// Truncates text and discard any partial words left at the end
        /// </summary>
        public static string TruncateWords(this string text, int maxCharacters, string trailingText)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
                return text;

            // trunctate the text, then remove the partial word at the end
            return Regex.Replace(text.Truncate(maxCharacters),
                @"\s+[^\s>]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled) + trailingText;

        } // End of the TruncateWords method

    } // End of the class

} // End of the namespace