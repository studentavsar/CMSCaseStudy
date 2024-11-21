using MediatR;
using ContentService.Models;
using ContentService.Queries;
using ContentService.Data;

namespace ContentService.Handlers
{
    public class GetContentByIdQueryHandler : IRequestHandler<GetContentByIdQuery, Content>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetContentByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Content> Handle(GetContentByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Contents.GetContentByIdAsync(request.Id);
        }
    }
}
