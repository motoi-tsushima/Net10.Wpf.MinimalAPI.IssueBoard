using System.Windows.Input;
using Net10.Wpf.MinimalAPI.IssueBoard.Services;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;

public class IssueDeleteViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly IssueApiService _apiService;
    private readonly int _issueId;

    private IssueDto? _issue;
    public IssueDto? Issue
    {
        get => _issue;
        set => SetProperty(ref _issue, value);
    }

    public ICommand DeleteCommand { get; }
    public ICommand BackCommand   { get; }

    public IssueDeleteViewModel(MainViewModel main, IssueApiService apiService, int issueId)
    {
        _main       = main;
        _apiService = apiService;
        _issueId    = issueId;

        DeleteCommand = new RelayCommand(async () => await DeleteAsync());
        BackCommand   = new RelayCommand(() => _main.NavigateToDetail(_issueId));

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Issue = await _apiService.GetByIdAsync(_issueId);
    }

    private async Task DeleteAsync()
    {
        try
        {
            await _apiService.DeleteAsync(_issueId);
            _main.NavigateToList();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"削除に失敗しました。\n{ex.Message}", "エラー",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
        }
    }
}
