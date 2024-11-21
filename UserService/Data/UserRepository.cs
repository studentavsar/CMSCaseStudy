using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(UserDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all contents.");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving content by id.");
                throw;
            }
        }

        public async Task AddUserAsync(User content)
        {
            try
            {
                await _context.Users.AddAsync(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new content.");
                throw;
            }
        }

        public async Task UpdateUserAsync(User content)
        {
            try
            {
                _context.Users.Update(content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the content.");
                throw;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                var content = await _context.Users.FindAsync(id);
                if (content != null)
                {
                    _context.Users.Remove(content);
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
