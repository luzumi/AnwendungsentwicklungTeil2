using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Web;
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
    /// Interaktionslogik für BindingCommands.xaml
    /// </summary>
    public partial class BindingCommands : Page
    {
        private string editBoxVal;
        private bool editBoxDirty;
        private bool isAlert = false;

        public string EditFieldValue
        {
            get
            {
                return editBoxVal;
            }
            set
            {
                if (editBoxVal != value)
                {
                    editBoxDirty = true;
                    editBoxVal = value;
                }
            }
        }

        public BindingCommands()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SaveExecuted(object pSender, ExecutedRoutedEventArgs pE)
        {
            MessageBox.Show("Es wurde gespeichert", "Hat geklappt", MessageBoxButton.OK, MessageBoxImage.Information);
            editBoxDirty = false;
        }

        private void SaveCanExecute(object pSender, CanExecuteRoutedEventArgs pE)
        {
            pE.CanExecute = editBoxDirty;
        }

        private void RedAlert_Executed(object pSender, ExecutedRoutedEventArgs pE)
        {
            isAlert = !isAlert;
            Background = isAlert? Brushes.Red : Brushes.White;
        }

        Window subWindow;
        private void NewWindow(object pSender, ExecutedRoutedEventArgs pE)
        {
            subWindow = new MenuWindow();
            subWindow.Show();
        }
    }
}
