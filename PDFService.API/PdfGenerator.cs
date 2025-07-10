using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Playwright;
using PDFService.API.Models;
using PDFService.API.PdfTemplates;

namespace PDFService.API
{
    public class PdfGenerator
    {
        private readonly HtmlRenderer _htmlRender;
        public PdfGenerator(HtmlRenderer htmlRender) => _htmlRender = htmlRender;

        private InvoiceItem[] GenerateInvoiceItems() => new[]
        {
            new InvoiceItem{ UnitPrice = 2000, Description = "Acer Laptop", Quantity = 5 },
            new InvoiceItem{ UnitPrice = 3000, Description = "Asus Laptop", Quantity = 2 },
            new InvoiceItem{ UnitPrice = 3000, Description = "Sony A7IV", Quantity = 1 },
            new InvoiceItem{ UnitPrice = 700, Description = "PS 5", Quantity = 1 },
        };

        public async Task<byte[]> RenderPDF()
        {
            byte[] pdf = default!;
            await _htmlRender.Dispatcher.InvokeAsync(async () =>
            {
                var dictionary = new Dictionary<string, object?>()
                {
                    { "InvoiceNumber", "007" },
                    { "CustomerName", "Kingsley" },
                    { "Phone", "000-111-222-3344" },
                    { "Address", "Planet Earth" },
                    { "Date", DateTime.Now },
                    { "Items", GenerateInvoiceItems() },
                };

                var parameters = ParameterView.FromDictionary(dictionary);
                var output = await _htmlRender.RenderComponentAsync<Invoice>(parameters);
                using var playwright = await Playwright.CreateAsync();
                var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true,
                });

                var page = await browser.NewPageAsync();
                var html = output.ToHtmlString();
                await page.SetContentAsync(html);
                pdf = await page.PdfAsync(new PagePdfOptions
                {
                    Format = "A4",
                    Landscape = true,
                    Path = Path.Combine(Directory.GetCurrentDirectory(), "test.pdf")
                });
            });

            return pdf;
        }
    }
}
