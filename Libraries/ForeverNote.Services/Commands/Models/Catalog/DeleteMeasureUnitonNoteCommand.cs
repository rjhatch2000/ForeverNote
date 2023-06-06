using MediatR;

namespace ForeverNote.Services.Commands.Models.Catalog
{
    public class DeleteMeasureUnitOnNoteCommand : IRequest<bool>
    {
        public string MeasureUnitId { get; set; }
    }
}
