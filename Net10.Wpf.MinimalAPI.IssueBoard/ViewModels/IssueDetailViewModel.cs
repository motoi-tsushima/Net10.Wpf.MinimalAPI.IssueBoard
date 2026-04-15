using System.Windows.Input;
using Net10.Wpf.MinimalAPI.IssueBoard.Services;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;

public class IssueDetailViewModel : ViewModelBase
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

    public ICommand EditCommand   { get; }
    public ICommand DeleteCommand { get; }
    public ICommand BackCommand   { get; }

    public IssueDetailViewModel(MainViewModel main, IssueApiService apiService, int issueId)
    {
        _main       = main;
        _apiService = apiService;
        _issueId    = issueId;

        EditCommand   = new RelayCommand(() => _main.NavigateToEdit(_issueId));
        DeleteCommand = new RelayCommand(() => _main.NavigateToDelete(_issueId));
        BackCommand   = new RelayCommand(() => _main.NavigateToList());

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Issue = await _apiService.GetByIdAsync(_issueId);
    }
}
