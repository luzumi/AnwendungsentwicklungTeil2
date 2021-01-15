using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaktionslogik für BindingPropertys.xaml
    /// </summary>
    public partial class BindingPropertys : Page, INotifyPropertyChanged
    {
        
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private double _sliderVal;

        private double _sliderVal2;

        private double _sliderVal3;

        public double PushBackSlider 
        {
            get
            {
                return _sliderVal2;
            }
            set
            {
                if (Math.Abs(_sliderVal2 - value) > 0.2)
                {
                    _sliderVal2 = value;
                    PushBackSlider2 = 100 - value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PushBackSlider"));
                    
                }
            }
        }

        public double PushBackSlider2 
        {
            get
            {
                
                return _sliderVal3;
            }
            set
            {
                if (Math.Abs(_sliderVal3 - value) > 0.1)
                {
                    _sliderVal3 = (int)value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PushBackSlider2"));
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SliderValue"));
                }
            }
        }

        public double SliderValue
        {
            get
            {
                return _sliderVal;
            }
            set
            {
                if (Math.Abs(_sliderVal - value) > 0.1)
                {
                    PushBackSlider = 100 - value;
                    _sliderVal = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SliderValue"));
                }
            }
        }


        public BindingPropertys()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnReadProp_Click(object pSender, RoutedEventArgs pE) => (pSender as Button).Content = SliderValue;

    }
}
