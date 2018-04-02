namespace GenealogyApp.Converters
{
    using GenealogyLogic.Interfaces;
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NewPersonToBooleanConverter : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || (value is IPerson && ((IPerson)value).FirstName == "VN" && ((IPerson)value).LastName == "NN") ? false : true;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || (value is IPerson && ((IPerson)value).FirstName == "VN" && ((IPerson)value).LastName == "NN") ? false : true;
        }
    }

}
