using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDataManagement.Utilities
{
    public static class StringExtensions
    {
        public static string[] SplitQuoted(this string input, char separator, char quotechar)
        {
            List<string> tokens = new List<string>();

            StringBuilder sb = new StringBuilder();
            bool escaped = false;
            foreach (char c in input)
            {
                if (c.Equals(separator) && !escaped)
                {
                    // we have a token
                    tokens.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else if (c.Equals(separator) && escaped)
                {
                    // ignore but add to string
                    sb.Append(c);
                }
                else if (c.Equals(quotechar))
                {
                    escaped = !escaped;
                    sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
            tokens.Add(sb.ToString().Trim());

            return tokens.ToArray();
        }

        public static string[] SplitQuoted(this string input, char separator)
        {
            List<string> tokens = new List<string>();
            char quotechar = '\"';

            StringBuilder sb = new StringBuilder();
            bool escaped = false;
            foreach (char c in input)
            {
                if (c.Equals(separator) && !escaped)
                {
                    // we have a token
                    tokens.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else if (c.Equals(separator) && escaped)
                {
                    // ignore but add to string
                    sb.Append(c);
                }
                else if (c.Equals(quotechar))
                {
                    escaped = !escaped;
                    sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
            tokens.Add(sb.ToString().Trim());

            return tokens.ToArray();
        }

        public static List<string> SplitQuotedList(this string input, char separator, char quotechar)
        {
            List<string> tokens = new List<string>();

            StringBuilder sb = new StringBuilder();
            bool escaped = false;
            foreach (char c in input)
            {
                if (c.Equals(separator) && !escaped)
                {
                    // we have a token
                    tokens.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else if (c.Equals(separator) && escaped)
                {
                    // ignore but add to string
                    sb.Append(c);
                }
                else if (c.Equals(quotechar))
                {
                    escaped = !escaped;
                    sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
            tokens.Add(sb.ToString().Trim());

            return tokens;
        }

        public static List<string> SplitQuotedList(this string input, char separator)
        {
            List<string> tokens = new List<string>();
            char quotechar = '\"';

            StringBuilder sb = new StringBuilder();
            bool escaped = false;
            foreach (char c in input)
            {
                if (c.Equals(separator) && !escaped)
                {
                    // we have a token
                    tokens.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                else if (c.Equals(separator) && escaped)
                {
                    // ignore but add to string
                    sb.Append(c);
                }
                else if (c.Equals(quotechar))
                {
                    escaped = !escaped;
                    sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                }
            }
            tokens.Add(sb.ToString().Trim());

            return tokens;
        }
    }
}
