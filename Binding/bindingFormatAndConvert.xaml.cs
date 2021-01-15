using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Binding
{
    /// <summary>
    /// Interaktionslogik für BindingFormatAndConvert.xaml
    /// </summary>
    public partial class BindingFormatAndConvert : Page
    {
        public BindingFormatAndConvert()
        {
            InitializeComponent();
            (FindResource("SizeToLine") as SizeToLine).ParentPage = cvsDiagramm;
            sld0.Value = 29;
            sld1.Value = 79;
            sld2.Value = 59;
        }
    }

    public class StringToBoolConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string && value.ToString().ToLower() == "ja")
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value == true)
            {
                return "ja";
            }

            return "nein";
        }

        #endregion
    }

    public class doubleToIntConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false) throw new NullReferenceException();
            return (double)value/30;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public class SizeToLine : IValueConverter
    {
        #region Implementation of IValueConverter

        public Canvas ParentPage;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double == false) throw new NullReferenceException();
            return (ParentPage != null ? ParentPage.ActualHeight - (double)value/4 - 50 : (double)0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
