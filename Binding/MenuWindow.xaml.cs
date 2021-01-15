using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Binding
{
    /// <summary>
    /// Interaktionslogik für MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void Close_Executed(object pSender, ExecutedRoutedEventArgs pE)
        {
            this.Close();
        }

        private void Save_Executed(object pSender, ExecutedRoutedEventArgs pE)
        {
            pE.Command.CanExecute(editBoxDirty); 
        }

        Window subWindow;
        private object editBoxDirty;

        private void NewWindow(object pSender, ExecutedRoutedEventArgs pE)
        {
            subWindow = new MenuWindow();
            subWindow.Show();
        }
    }
}
