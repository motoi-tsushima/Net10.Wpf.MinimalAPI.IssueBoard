using Api.MinimalAPI.IssueBoard.Models;
using Shared.Rest.IssueBoard.Dtos;

namespace Api.MinimalAPI.IssueBoard.Mapping;

public static class IssueMapping
{
    public static IssueDto ToDto(this Issue issue) => new()
    {
        Id           = issue.Id,
        AuthorName   = issue.AuthorName,
        CreatedAt    = issue.CreatedAt,
        Category     = issue.Category,
        Title        = issue.Title,
        Description  = issue.Description,
        Status       = (IssueStatus)issue.Status,
        Resolution   = issue.Resolution,
        ResolverName = issue.ResolverName,
        ResolvedAt   = issue.ResolvedAt,
    };

    public static Issue ToModel(this IssueCreateDto dto) => new()
    {
        AuthorName  = dto.AuthorName.Trim(),
        CreatedAt   = DateTime.Now,
        Category    = string.IsNullOrWhiteSpace(dto.Category) ? null : dto.Category.Trim(),
        Title       = dto.Title.Trim(),
        Description = dto.Description.Trim(),
        Status      = 0,
    };

    public static void UpdateModel(this Issue issue, IssueUpdateDto dto)
    {
        issue.Category     = string.IsNullOrWhiteSpace(dto.Category)     ? null : dto.Category.Trim();
        issue.Title        = dto.Title.Trim();
        issue.Description  = dto.Description.Trim();
        issue.Status       = (int)dto.Status;
        issue.Resolution   = string.IsNullOrWhiteSpace(dto.Resolution)   ? null : dto.Resolution.Trim();
        issue.ResolverName = string.IsNullOrWhiteSpace(dto.ResolverName) ? null : dto.ResolverName.Trim();
        issue.ResolvedAt   = DateTime.Now;
    }
}
