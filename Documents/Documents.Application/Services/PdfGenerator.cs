using Documents.Application.Exceptions;
using Documents.Application.Services.Abstractions;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;
using System.IO;
using System.Threading.Tasks;

namespace Documents.Application.Services
{
    public static class PdfGenerator : IPdfGenerator
    {
        public async static Task<byte[]> GenerateFile(string text, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new BadRequestException("Text cannot be empty.");
            }

            using var memoryStream = new MemoryStream();

            var writer = new PdfWriter(memoryStream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            document.Add(new Paragraph(text));
            document.Close();

            var pdfBytes = memoryStream.ToArray();
            return await Task.FromResult(pdfBytes);
        }
    }
}
