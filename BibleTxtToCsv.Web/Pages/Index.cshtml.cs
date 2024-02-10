using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BibleTxtToCsv.Web.Pages
{
    public class ConvertModel : PageModel
    {
        private readonly ILogger<ConvertModel> _logger;
        [BindProperty]
        public string? Text { get; set; }
        public string? Csv { get; set; }

        public ConvertModel(ILogger<ConvertModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            Text = @"James 1
1 James, a servant of God and of the Lord Jesus Christ, To the twelve tribes scattered among the nations: Greetings.
2 Consider it pure joy, my brothers and sisters, whenever you face trials of many kinds,";
            Csv = @"Book,Chapter,Verse,Reference,Text
""James"",1,1,""James 1:1"",""James, a servant of God and of the Lord Jesus Christ, To the twelve tribes scattered among the nations: Greetings.""
""James"",1,2,""James 1:2"",""Consider it pure joy, my brothers and sisters, whenever you face trials of many kinds,""";

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public IActionResult OnPostAsync()
        {
            if (Text is null)
            {
                Csv = "No Text Provided";
            }
            else
            {
                try
                {
                    Csv = BibleTxtToCsv.Lib.TxtToCsv.Convert(Text);
                }
                catch (Exception ex)
                {
                    Csv = ex.Message;
                }
            }

            return Page();
        }
    }
}
