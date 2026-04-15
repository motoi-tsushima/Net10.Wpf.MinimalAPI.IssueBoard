using System.Windows;
using Net10.Wpf.MinimalAPI.IssueBoard.ViewModels;

namespace Net10.Wpf.MinimalAPI.IssueBoard
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
