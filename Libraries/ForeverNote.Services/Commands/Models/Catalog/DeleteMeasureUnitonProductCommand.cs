using MediatR;

namespace ForeverNote.Services.Commands.Models.Catalog
{
    public class DeleteMeasureUnitOnProductCommand : IRequest<bool>
    {
        public string MeasureUnitId { get; set; }
    }
}
