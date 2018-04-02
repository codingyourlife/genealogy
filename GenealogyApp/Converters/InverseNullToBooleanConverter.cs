namespace GenealogyApp.Converters
{
    using System;
    using System.Globalization;

    public class InverseNullToBooleanConverter : NullToBooleanConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseResult = (bool)base.Convert(value, targetType, parameter, culture);
            return !baseResult;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var baseResult = !(bool)base.ConvertBack(value, targetType, parameter, culture);
            return baseResult;
        }
    }

}
