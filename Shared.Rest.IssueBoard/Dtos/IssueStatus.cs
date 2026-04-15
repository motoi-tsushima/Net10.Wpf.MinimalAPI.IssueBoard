namespace Shared.Rest.IssueBoard.Dtos;

public enum IssueStatus
{
    NotStarted    = 0,  // 未着手
    InProgress    = 1,  // 着手中
    Failed        = 2,  // 解決失敗
    CannotConfirm = 3,  // 課題確認不能
    Resolved      = 4   // 解決済み
}
