namespace Shared.Rest.IssueBoard.Dtos;

public class IssueCreateDto
{
    public string AuthorName { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
