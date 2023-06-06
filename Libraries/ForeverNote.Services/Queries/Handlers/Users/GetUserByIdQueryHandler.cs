using ForeverNote.Core.Data;
using ForeverNote.Core.Domain.Users;
using ForeverNote.Services.Queries.Models.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ForeverNote.Services.Queries.Handlers.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IRepository<User> _userRepository;

        public GetUserQueryHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
                return Task.FromResult<User>(null);

            return _userRepository.GetByIdAsync(request.Id);
        }
    }
}
