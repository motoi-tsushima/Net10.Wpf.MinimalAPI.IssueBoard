using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.Helpers;

public static class IssueStatusHelper
{
    public static string GetDisplayName(IssueStatus status) => status switch
    {
        IssueStatus.NotStarted    => "未着手",
        IssueStatus.InProgress    => "着手中",
        IssueStatus.Failed        => "解決失敗",
        IssueStatus.CannotConfirm => "課題確認不能",
        IssueStatus.Resolved      => "解決済み",
        _                         => status.ToString()
    };

    public static IEnumerable<IssueStatusItem> GetAllItems() =>
        Enum.GetValues<IssueStatus>().Select(s => new IssueStatusItem(s, GetDisplayName(s)));
}

public record IssueStatusItem(IssueStatus Value, string DisplayName);
