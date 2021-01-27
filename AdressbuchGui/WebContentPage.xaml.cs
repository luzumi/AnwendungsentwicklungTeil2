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
using AdressbuchLogic;

namespace AdressbuchGui
{
    /// <summary>
    /// Interaktionslogik für WebContentPage.xaml
    /// </summary>
    public partial class WebContentPage
    {
        private ObservableCollection<UserViewModel> Items;

        /*public WebContentPage(Object pDataContext)
        {
            InitializeComponent();
            DataContext = pDataContext;
        }*/

        public WebContentPage() 
        {
            InitializeComponent(); 
            /*DataContext = this;
            Items = new();
            Items.Add(new UserViewModel("https://www.reddit.com/r/Luzumi/"));*/
        }



        private void BrowserNavigateTo(object pSender, NavigatingCancelEventArgs pNavigatingCancelEventArgs)
        {
            Items.Add(new UserViewModel(pNavigatingCancelEventArgs.Uri.OriginalString));
        }

        private void ButtonReddit_OnClick(object pSender, RoutedEventArgs pE)
        {
            (DataContext as AdressbuchViewModel).InternetAdress = String.Format($"https://www.reddit.com/r/{lblReddit.Content}/");
        }

        private void ButtonInsragram_OnClick(object pSender, RoutedEventArgs pE)
        {
            (DataContext as AdressbuchViewModel).InternetAdress = "https://www.instagram.com/?hl=de";
        }

        private void ButtonXing_OnClick(object pSender, RoutedEventArgs pE)
        {
            ChromWebBrowser.Address = "https://www.xing.com/";
        }
        /*
        private void ButtonLinkedIn_OnClick(object pSender, RoutedEventArgs pE)
        {
            if (lblFirstName != null && lblLastName != null)
            {
                ChromWebBrowser.Address =
                    $"https://de.linkedin.com/pub/dir?firstName={lblFirstName.Content}&lastName={lblLastName.Content}&trk=public_profile_people-search-bar_search-submit";
            }
            else if (lblLinkedIn != null)
            {
                ChromWebBrowser.Address = "https://de.linkedin.com";
            }
        }
        */
        private void ButtonFacebook_OnClick(object pSender, RoutedEventArgs pE)
        {
            ChromWebBrowser.Address = "https://de-de.facebook.com";
        }

        private void ButtonTwitter_OnClick(object pSender, RoutedEventArgs pE)
        {
            ChromWebBrowser.Address = "https://twitter.com/?lang=de";
        }

        private void ButtonEmail_OnClick(object pSender, RoutedEventArgs pE)
        {
            ChromWebBrowser.Address = $"https://www.reddit.com/r/Luzumi/";
        }
    }
}
