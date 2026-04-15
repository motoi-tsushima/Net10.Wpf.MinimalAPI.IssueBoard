using Net10.Wpf.MinimalAPI.IssueBoard.Services;

namespace Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IssueApiService _apiService;
    private ViewModelBase _currentView = null!;

    public ViewModelBase CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    public MainViewModel()
    {
        _apiService = new IssueApiService("http://localhost:5044");
        NavigateToList();
    }

    public void NavigateToList()   => CurrentView = new IssueListViewModel(this, _apiService);
    public void NavigateToDetail(int id) => CurrentView = new IssueDetailViewModel(this, _apiService, id);
    public void NavigateToEdit(int id)   => CurrentView = new IssueEditViewModel(this, _apiService, id);
    public void NavigateToDelete(int id) => CurrentView = new IssueDeleteViewModel(this, _apiService, id);
    public void NavigateToCreate() => CurrentView = new IssueCreateViewModel(this, _apiService);
}
