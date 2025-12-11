using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ActiviaAPP.Classes
{       
    public class RegisteredToVisibilityConverter : IMultiValueConverter
    {
        //Konverterer en samling af registrerede brugere og et aktuelt bruger-ID til Visibility
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //Hvis der mangler værdier, skjules disse elementer
            if (values == null || values.Length < 2)
                return Visibility.Collapsed;

            //Henter den registrerede brugerliste
            var list = values[0] as ObservableCollection<string>;

            //Henter det aktuelle bruger-ID
            var currentUserId = values[1] as string;

            //Hvis listen eller bruger-ID'et er ugyldigt, skjules elementet
            if (list == null || string.IsNullOrWhiteSpace(currentUserId))
                return Visibility.Collapsed;

            //Viser elementet hvis den aktuelle bruger er registreret, ellers skjules det
            return list.Contains(currentUserId) ? Visibility.Visible : Visibility.Collapsed;
        }

        //Konvertering tilbage understøttes ikke
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}