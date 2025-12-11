using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ActiviaAPP.Classes
{       
    public class RegisteredToVisibilityConverter : IMultiValueConverter
    {   

        /// <summary>
        /// Kodet af alle
        /// </summary>

        //Konverterer en samling af registrerede brugere og et aktuelt bruger-ID til Visibility
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //Hvis der ikke er nok værdier, skjul elementet
            if (values == null || values.Length < 2)
            {
                return Visibility.Collapsed;
            }

            //Hent listen af registrerede brugere
            ObservableCollection<string> registeredUsers = values[0] as ObservableCollection<string>;

            //Hent det aktuelle bruger-ID
            string currentUserId = values[1] as string;

            //Hvis listen er null eller bruger-ID er tomt, skjul elementet
            if (registeredUsers == null || currentUserId == null || currentUserId == "")
            {
                return Visibility.Collapsed;
            }

            //Tjek om den aktuelle bruger er i listen
            bool isRegistered = false;

            //Loop gennem alle registrerede brugere
            for (int i = 0; i < registeredUsers.Count; i++)
            {
                //Hvis bruger-ID'et matcher
                if (registeredUsers[i] == currentUserId)
                {
                    isRegistered = true;
                    break;
                }
            }

            //Hvis brugeren er registreret, vis elementet, ellers skjul det
            if (isRegistered)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        //Konvertering tilbage understøttes ikke
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}