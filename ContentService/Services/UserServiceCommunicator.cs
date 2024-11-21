public interface IUserServiceCommunicator
{
    Task<bool> UserExistsAsync(int userId);
}

public class UserServiceCommunicator : IUserServiceCommunicator
{
    private readonly HttpClient _httpClient;

    public UserServiceCommunicator(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"/api/users/{userId}");
        return response.IsSuccessStatusCode;
    }
}
