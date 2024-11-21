using MediatR;
using ContentService.Commands;
using ContentService.Data;

namespace ContentService.Handlers
{
    public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.Contents.DeleteContentAsync(request.Id);
            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }

    }
}
