namespace GenealogyApp.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;

    public class InverseNewPersonToVisbilityConverter : NewPersonToBooleanConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseResult = !(bool)base.Convert(value, targetType, parameter, culture);
            return baseResult ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseResult = !(bool)base.ConvertBack(value, targetType, parameter, culture);
            return baseResult ? Visibility.Collapsed : Visibility.Visible;
        }
    }

}
