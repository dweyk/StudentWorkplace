namespace StudentWorkplace.Assets.Converters;

public class RoleToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		// Проверяем, что значение роли совпадает с ожидаемым
		if (value != null! && value.ToString() == "Admin")
		{
			return Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}