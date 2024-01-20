using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleTxtToCsv.Lib
{
    public static class TxtToCsv
    {
        public static string Convert(string txt)
        {
            txt = txt.Replace("�", " ");
            txt = txt.Replace("\u00A0", " ");
            var verses = new List<Verse>();
            string book = "Unknown";
            int chapter = 0;
            var lines = txt.Split(Environment.NewLine);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                // See if the line starts with a number
                var spaceIndex = line.IndexOf(" ");
                if (spaceIndex >= 0)
                {
                    // See if firstToken is a number
                    if (int.TryParse(line.Substring(0, spaceIndex), out int verseNumber))
                    {
                        // This is a verse
                        verses.Add(new Verse(book, chapter, verseNumber, line.Substring(spaceIndex + 1)));
                    }
                    else
                    {
                        // This is a book and chapter number.
                        var parts = line.Split(" ");
                        // Make sure the last part is a number
                        if (int.TryParse(parts[parts.Length - 1], out var tempChapter))
                        {
                            if (tempChapter > 0)
                            {
                                chapter = tempChapter;
                                book = string.Join(' ', parts.Take(parts.Length - 1));
                            }
                        }
                        else
                        {
                            // Going to ignore this line in case someone doesn't take out the inserted headings.
                            // Put the next line back in if you want to throw an exception if a line isn't either a Book and Chapter or a Verse.
                            // if (parts.Length != 2) throw new FormatException("A chapter heading must be formatted as [book] [chapter] 'Second Kings 4");
                        }
                    }
                }
            }
            StringBuilder result = new();

            result.AppendLine("Book,Chapter,Verse,Text");
            verses.ForEach(f => result.AppendLine(f.FormatCsv()));

            return result.ToString();
        }
    }
}
