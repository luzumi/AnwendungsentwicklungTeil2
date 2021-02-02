using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace AdressbuchLogic
{
    class InternetStringConverter : IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter

        public object Convert(object[] pValues, Type pTargetType, object pArameter,
            CultureInfo pCulture)
        {
            return (pValues[0] as Button)?.Name switch
            {
                "LinkedInButton" =>
                    $"https://de.linkedin.com/pub/dir?firstName={pValues[1]}&lastName={pValues[2]}&trk=public_profile_people-search-bar_search-submit",
                "FacebookButton" => $"https://de-de.facebook.com/",
                "XingButton" => $"https://www.xing.com/",
                "TwitterButton" => $"https://twitter.com/?lang=de",
                "InstagramButton" => $"https://www.instagram.com/?hl=de",
                "RedditButton" => $"https://www.reddit.com/r/{pValues[1]}/",
                "emailButton" => "",
                _ => ""
            };
        }

        public object[] ConvertBack(object pValue, Type[] pTargetTypes, object pArameter, CultureInfo pCulture)
        {
            return null;
        }

        #endregion
    }
}
