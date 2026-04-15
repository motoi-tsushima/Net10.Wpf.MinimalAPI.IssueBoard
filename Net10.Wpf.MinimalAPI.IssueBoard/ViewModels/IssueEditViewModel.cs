using System.Windows.Input;
using Net10.Wpf.MinimalAPI.IssueBoard.Helpers;
using Net10.Wpf.MinimalAPI.IssueBoard.Services;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;

public class IssueEditViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly IssueApiService _apiService;
    private readonly int _issueId;

    public string AuthorName      { get; private set; } = string.Empty;
    public string CreatedAtDisplay { get; private set; } = string.Empty;

    private string _category = string.Empty;
    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private IssueStatus _selectedStatus;
    public IssueStatus SelectedStatus
    {
        get => _selectedStatus;
        set => SetProperty(ref _selectedStatus, value);
    }

    private string _resolution = string.Empty;
    public string Resolution
    {
        get => _resolution;
        set => SetProperty(ref _resolution, value);
    }

    private string _resolverName = string.Empty;
    public string ResolverName
    {
        get => _resolverName;
        set => SetProperty(ref _resolverName, value);
    }

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public IEnumerable<IssueStatusItem> StatusItems { get; } = IssueStatusHelper.GetAllItems().ToList();

    public ICommand UpdateCommand { get; }
    public ICommand BackCommand   { get; }

    public IssueEditViewModel(MainViewModel main, IssueApiService apiService, int issueId)
    {
        _main       = main;
        _apiService = apiService;
        _issueId    = issueId;

        UpdateCommand = new RelayCommand(async () => await UpdateAsync());
        BackCommand   = new RelayCommand(() => _main.NavigateToDetail(_issueId));

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        var issue = await _apiService.GetByIdAsync(_issueId);
        if (issue is null) return;

        AuthorName       = issue.AuthorName;
        CreatedAtDisplay = issue.CreatedAt.ToString("yyyy/MM/dd HH:mm:ss");
        Category         = issue.Category     ?? string.Empty;
        Title            = issue.Title;
        Description      = issue.Description;
        SelectedStatus   = issue.Status;
        Resolution       = issue.Resolution   ?? string.Empty;
        ResolverName     = issue.ResolverName ?? string.Empty;

        OnPropertyChanged(nameof(AuthorName));
        OnPropertyChanged(nameof(CreatedAtDisplay));
    }

    private async Task UpdateAsync()
    {
        ErrorMessage = Validate();
        if (!string.IsNullOrEmpty(ErrorMessage)) return;

        var dto = new IssueUpdateDto
        {
            Category     = Category,
            Title        = Title,
            Description  = Description,
            Status       = SelectedStatus,
            Resolution   = Resolution,
            ResolverName = ResolverName,
        };

        try
        {
            await _apiService.UpdateAsync(_issueId, dto);
            _main.NavigateToDetail(_issueId);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"更新に失敗しました。\n{ex.Message}";
        }
    }

    private string Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))       return "課題タイトルは必須です。";
        if (Title.Length > 100)                     return "課題タイトルは100文字以内で入力してください。";
        if (Category.Length > 30)                   return "カテゴリ名は30文字以内で入力してください。";
        if (string.IsNullOrWhiteSpace(Description)) return "課題の文面は必須です。";
        if (Description.Length > 2000)              return "課題の文面は2000文字以内で入力してください。";
        if (Resolution.Length > 2000)               return "解決の文面は2000文字以内で入力してください。";
        if (ResolverName.Length > 50)               return "解決担当者名は50文字以内で入力してください。";
        return string.Empty;
    }
}
