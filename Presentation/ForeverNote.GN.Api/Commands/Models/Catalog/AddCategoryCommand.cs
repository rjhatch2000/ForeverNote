using ForeverNote.Api.DTOs.Catalog;
using MediatR;

namespace ForeverNote.Api.Commands.Models.Catalog
{
    public class AddCategoryCommand : IRequest<CategoryDto>
    {
        public CategoryDto Model { get; set; }
    }
}
