using System.Windows.Controls;
using System.Windows.Input;
using Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.Views;

public partial class IssueListView : UserControl
{
    public IssueListView()
    {
        InitializeComponent();
    }

    private void DataGridRow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (sender is DataGridRow row
            && row.DataContext is IssueDto dto
            && DataContext is IssueListViewModel vm)
        {
            vm.GoToDetailCommand.Execute(dto);
        }
    }
}
