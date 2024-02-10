// This takes a simple text file of Bible verses and converts it to a csv file

// Format
// [Book] [Chapter Number]
// [Verse] [Verse Text]
// [Verse] [Verse Text]
// [Book] [Chapter Number]
// [Verse] [Verse Text]
// [Verse] [Verse Text]

// Text can be easily taken from here: https://www.biblestudytools.com/james/1.html

using BibleTxtToCsv.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Text;

// Determine the filename on the command line
if (args.Length != 1) throw new ArgumentException("The input filename must be specified");
var txtFilename = args[0];

var txt = File.ReadAllText(txtFilename);
txt = txt.Replace("�", " ");
var verses = new List<Verse>();
string book = "Unknown";
int chapter = 0;
var lines = txt.Split(Environment.NewLine);
Console.WriteLine($"Reading {lines.Length} lines from {txtFilename}.");
foreach (var line in lines)
{
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
            if (int.TryParse(parts[parts.Length - 1], out chapter))
            {
                book = string.Join(' ', parts.Take(parts.Length - 1));
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
var filenameParts = txtFilename.Split(".");
var csvFilename = string.Join(',', filenameParts.Take(filenameParts.Length - 1)) + ".csv";

File.WriteAllText(csvFilename, "Book,Chapter,Verse,Reference,Text" + Environment.NewLine);
File.AppendAllLines(csvFilename, verses.Select(f => f.FormatCsv()));

Console.WriteLine($"Wrote {verses.Count} verses to {csvFilename}.");


