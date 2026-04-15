using System.Windows.Input;
using Net10.Wpf.MinimalAPI.IssueBoard.Services;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;

public class IssueCreateViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly IssueApiService _apiService;

    private string _authorName = SettingsService.GetAuthorName();
    public string AuthorName
    {
        get => _authorName;
        set => SetProperty(ref _authorName, value);
    }

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

    private string _errorMessage = string.Empty;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand RegisterCommand { get; }
    public ICommand BackCommand     { get; }

    public IssueCreateViewModel(MainViewModel main, IssueApiService apiService)
    {
        _main       = main;
        _apiService = apiService;

        RegisterCommand = new RelayCommand(async () => await RegisterAsync());
        BackCommand     = new RelayCommand(() => _main.NavigateToList());
    }

    private async Task RegisterAsync()
    {
        ErrorMessage = Validate();
        if (!string.IsNullOrEmpty(ErrorMessage)) return;

        var dto = new IssueCreateDto
        {
            AuthorName  = AuthorName.Trim(),
            Category    = Category,
            Title       = Title.Trim(),
            Description = Description.Trim(),
        };

        try
        {
            await _apiService.CreateAsync(dto);
            SettingsService.SaveAuthorName(dto.AuthorName);
            _main.NavigateToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"登録に失敗しました。\n{ex.Message}";
        }
    }

    private string Validate()
    {
        if (string.IsNullOrWhiteSpace(AuthorName)) return "記入者氏名は必須です。";
        if (AuthorName.Length > 50)                return "記入者氏名は50文字以内で入力してください。";
        if (Category.Length > 30)                  return "カテゴリ名は30文字以内で入力してください。";
        if (string.IsNullOrWhiteSpace(Title))      return "課題タイトルは必須です。";
        if (Title.Length > 100)                    return "課題タイトルは100文字以内で入力してください。";
        if (string.IsNullOrWhiteSpace(Description)) return "課題の文面は必須です。";
        if (Description.Length > 2000)             return "課題の文面は2000文字以内で入力してください。";
        return string.Empty;
    }
}
