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
            switch ((pValues[0] as Button)?.Name)
            {
                case "LinkedInButton":
                    return
                        $"https://de.linkedin.com/pub/dir?firstName={pValues[1]}&lastName={pValues[2]}&trk=public_profile_people-search-bar_search-submit";
                case "FacebookButton":
                    return $"https://de-de.facebook.com/";
                case "XingButton":
                    return $"https://www.xing.com/";
                case "TwitterButton":
                    return $"https://twitter.com/?lang=de";
                case "InstagramButton":
                    return $"https://www.instagram.com/?hl=de";
                case "RedditButton":
                    return $"https://www.reddit.com/r/{pValues[1]}/";
                case "emailButton":
                default:
                    return "";
            }
        }

        public object[] ConvertBack(object pValue, Type[] pTargetTypes, object pArameter, CultureInfo pCulture)
        {
            return null;
        }

        #endregion
    }
}
