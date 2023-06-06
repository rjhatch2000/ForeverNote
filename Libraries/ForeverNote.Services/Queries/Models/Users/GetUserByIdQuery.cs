using ForeverNote.Core.Domain.Users;
using MediatR;
namespace ForeverNote.Services.Queries.Models.Users
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public string Id { get; set; }
    }
}
