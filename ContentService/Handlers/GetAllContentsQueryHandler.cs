using MediatR;
using ContentService.Models;
using ContentService.Queries;
using ContentService.Data;

namespace ContentService.Handlers
{
    public class GetAllContentsQueryHandler : IRequestHandler<GetAllContentsQuery, IEnumerable<Content>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllContentsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Content>> Handle(GetAllContentsQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Contents.GetAllContentsAsync();
        }
    }
}
