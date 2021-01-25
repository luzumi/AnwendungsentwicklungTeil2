using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
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

namespace AdressbuchGui
{
    /// <summary>
    /// Interaktionslogik für WebContentPage.xaml
    /// </summary>
    public partial class WebContentPage : Page
    {
        private ObservableCollection<UserViewModer> Items;

        public WebContentPage(Object pDataContext)
        {
            InitializeComponent();
            DataContext = pDataContext;
        }

        public WebContentPage()
        {
            InitializeComponent();
            DataContext = this;
            Items = new();
            Items.Add(new UserViewModer("https://social.msdn.microsoft.com/Forums/en-US/491abb68-ad60-43f5-9923-246096be4b39/how-to-open-a-wpf-page-in-a-grid?forum=wpf"));
        }

        private void BrowserNavigateTo(object pSender, NavigatingCancelEventArgs pNavigatingCancelEventArgs)
        {
            Items.Add(new UserViewModer(pNavigatingCancelEventArgs.Uri.OriginalString));
        }

        
    }
}
