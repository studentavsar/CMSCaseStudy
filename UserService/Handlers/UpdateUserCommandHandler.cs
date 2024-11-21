using MediatR;
using UserService.Models;
using UserService.Commands;
using UserService.Data;

namespace UserService.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.Users.UpdateUserAsync(request.User);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
