using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Net10.Wpf.MinimalAPI.IssueBoard.Services;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;

public class IssueListViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly IssueApiService _apiService;

    private ObservableCollection<IssueDto> _issues = [];
    public ObservableCollection<IssueDto> Issues
    {
        get => _issues;
        set => SetProperty(ref _issues, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand GoToDetailCommand { get; }
    public ICommand GoToCreateCommand { get; }
    public ICommand ExitCommand       { get; }

    public IssueListViewModel(MainViewModel main, IssueApiService apiService)
    {
        _main       = main;
        _apiService = apiService;

        GoToDetailCommand = new RelayCommand<IssueDto>(dto =>
        {
            if (dto is not null) _main.NavigateToDetail(dto.Id);
        });
        GoToCreateCommand = new RelayCommand(() => _main.NavigateToCreate());
        ExitCommand       = new RelayCommand(() => Application.Current.Shutdown());

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            IsLoading = true;
            var items = await _apiService.GetAllAsync();
            Issues = new ObservableCollection<IssueDto>(items);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"データの取得に失敗しました。\n{ex.Message}", "エラー",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
