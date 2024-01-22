using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleTxtToCsv.Lib
{
    public class Verse
    {
        public Verse(string book, int chapter, int verseNumber, string text)
        {
            Book = book;
            Chapter = chapter;
            VerseNumber = verseNumber;
            Text = text;
        }

        public string Book { get; set; }
        public int Chapter { get; set; }
        public int VerseNumber { get; set; }
        public string Text { get; set; }

        public string FormatCsv()
        {
            return $@"""{Book}"",{Chapter},{VerseNumber},""{Text.Replace("\"","\"\"")}""";
        }
    }
}
