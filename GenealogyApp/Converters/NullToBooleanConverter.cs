namespace GenealogyApp.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NullToBooleanConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : true;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? false : true;
        }
    }

}
