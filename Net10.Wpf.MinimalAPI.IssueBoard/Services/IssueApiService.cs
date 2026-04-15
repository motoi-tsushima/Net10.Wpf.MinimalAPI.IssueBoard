using System.Net.Http;
using System.Net.Http.Json;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.Services;

public class IssueApiService
{
    private readonly HttpClient _httpClient;

    public IssueApiService(string baseUrl)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    public async Task<List<IssueDto>> GetAllAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<IssueDto>>("/api/issues") ?? [];
    }

    public async Task<IssueDto?> GetByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<IssueDto>($"/api/issues/{id}");
    }

    public async Task<IssueDto?> CreateAsync(IssueCreateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/issues", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IssueDto>();
    }

    public async Task<IssueDto?> UpdateAsync(int id, IssueUpdateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/issues/{id}", dto);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IssueDto>();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/issues/{id}");
        response.EnsureSuccessStatusCode();
    }
}
