using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace ConsoleLogHighlighter
{
    public class Highlighter : IEnumerable
    {
        private readonly Dictionary<string, string> m_patternKeys = new Dictionary<string, string>();
        private readonly Dictionary<string, string> m_styles = new Dictionary<string, string>();

        public void Add([RegexPattern] string regexPattern, string styleKey)
        {
            if (regexPattern == null) throw new ArgumentNullException(nameof(regexPattern));
            if (regexPattern == null) throw new ArgumentNullException(nameof(regexPattern));
            var testRegex = new Regex(regexPattern);
            m_patternKeys.Add(regexPattern, styleKey);
        }

        public void AddStyle(string styleKey, string style)
        {
            m_styles[styleKey] = style;
        }

        public string Transform(string input)
        {
            if (m_patternKeys.Count == 0) return input;

            var commonPattern = string.Join("|", m_patternKeys.Select(kvp =>
            {
                var pattern = kvp.Key;
                var styleKey = kvp.Value;
                return $"(?<{styleKey}>{pattern})";
            }));

            var regex = new Regex(commonPattern, RegexOptions.ExplicitCapture, TimeSpan.FromMilliseconds(100));

            return regex.Replace(input, Replacement);
        }

        private string Replacement(Match match)
        {
            var style = GetStyle(match);
            if (style != null)
            {
                return $"{style}{match.Value}\x1B[m";
            }

            return match.Value;
        }

        private string GetStyle(Match match)
        {
            if (match.Success)
            {
                foreach (Group group in match.Groups)
                {
                    if (group.Success && m_styles.TryGetValue(group.Name, out var style))
                    {
                        return style;
                    }
                }
            }

            return null;
        }

        public IEnumerator GetEnumerator()
        {
            yield break;
        }
    }
}
