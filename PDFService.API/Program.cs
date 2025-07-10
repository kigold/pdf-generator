using Microsoft.AspNetCore.Components.Web;
using PDFService.API;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Microsoft.Playwright.Program.Main(["install"]);

builder.Services.AddScoped<HtmlRenderer>();
builder.Services.AddScoped<PdfGenerator>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.MapScalarApiReference(endpointPrefix: "/", opt => opt.Servers = []);
}

app.UseHttpsRedirection();

app.MapGet("pdf", async (PdfGenerator service) =>
{
    var mimeType = "application/pdf";
    var pdf = await service.RenderPDF();
    return Results.File(pdf, contentType: mimeType);
})
.WithName("Generate Pdf invoice");

app.Run();

