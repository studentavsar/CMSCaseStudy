using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ContentService.Models;
using System.Net.Http;

namespace ContentService.Data
{
    public class ContentRepository : IContentRepository
    {
        private readonly ContentDbContext _context;
        private readonly ILogger<ContentRepository> _logger;

        public ContentRepository(ContentDbContext context, ILogger<ContentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Content>> GetAllContentsAsync()
        {
            try
            {
                return await _context.Contents.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all contents.");
                throw;
            }
        }

        public async Task<Content> GetContentByIdAsync(int id)
        {
            try
            {
                return await _context.Contents.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving content by id.");
                throw;
            }
        }

        public async Task AddContentAsync(Content content)
        {
            try
            {
                await _context.Contents.AddAsync(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new content.");
                throw;
            }
        }

        public async Task UpdateContentAsync(Content content)
        {
            try
            {
                _context.Contents.Update(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the content.");
                throw;
            }
        }

        public async Task DeleteContentAsync(int id)
        {
            try
            {
                var content = await _context.Contents.FindAsync(id);
                if (content != null)
                {
                    _context.Contents.Remove(content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the content.");
                throw;
            }
        }
    }
}
