using System.Globalization;
using System.Windows.Data;
using Net10.Wpf.MinimalAPI.IssueBoard.Helpers;
using Shared.Rest.IssueBoard.Dtos;

namespace Net10.Wpf.MinimalAPI.IssueBoard.Converters;

public class IssueStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IssueStatus status)
            return IssueStatusHelper.GetDisplayName(status);
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
