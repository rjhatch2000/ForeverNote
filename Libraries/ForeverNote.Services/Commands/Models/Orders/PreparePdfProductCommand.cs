using ForeverNote.Core.Domain.Catalog;
using ForeverNote.Core.Domain.Localization;
using iTextSharp.text.pdf;
using MediatR;

namespace ForeverNote.Services.Commands.Models.Orders
{
    public class PreparePdfProductCommand : IRequest<PdfPTable>
    {
        public Product Product { get; set; }
        public Language Language { get; set; }
        public int ProductNumber { get; set; }
    }
}
