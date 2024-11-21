using ContentService.Models;

namespace ContentService.Data
{
    public interface IContentRepository
    {
        Task<IEnumerable<Content>> GetAllContentsAsync();
        Task<Content> GetContentByIdAsync(int id);
        Task AddContentAsync(Content content);
        Task UpdateContentAsync(Content content);
        Task DeleteContentAsync(int id);
    }
}
