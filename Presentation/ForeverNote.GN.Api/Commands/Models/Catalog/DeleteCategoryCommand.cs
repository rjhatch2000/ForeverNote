using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class DeleteCategoryCommand : IRequest<bool>
    {
        public CategoryDto Model { get; set; }
    }
}
