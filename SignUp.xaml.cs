using ActiviaAPP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActiviaAPP
{
    //Kodet af Casper
    public partial class SignUp : Page
    {
        public SignUp()
        {
            InitializeComponent();
        }

        // Opretter en ny bruger når der trykkes på Opret bruger knappen
        private void CreateUser(object sender, RoutedEventArgs e)
        {
            // Tjekker om alle felter er udfyldt før man kan gå videre
            if (string.IsNullOrWhiteSpace(FullNameBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameBox.Text) ||
                string.IsNullOrWhiteSpace(EmailBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                // Hvis ikke alle felter er udfyldt, vises denne besked og funktionen stopper
                MessageBox.Show("Udfyld alle felter");
                return;
            }

            // Tjek om adgangskoderne matcher i oprettelsen
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                // Hvis adgangskoderne ikke er ens, vises denne besked og funktionen stopper
                MessageBox.Show("Adgangskoden matcher ikke");
                return;
            }


            // Tjekker om brugernavnet allerede findes
            if (UserStore.UsernameExists(UsernameBox.Text))
            {
                // Hvis brugernavnet allerede findes, vises denne besked og funktionen stopper
                MessageBox.Show("Brugernavnet er allerede taget");
                return;
            }


            // Opretter en ny bruger i klassen User.cs
            var newUser = new ActiviaAPP.Classes.User
            {
                //Angiver de skrevne værdierne til de forskellige attributter, for den nye bruger
                FullName = FullNameBox.Text,
                Username = UsernameBox.Text,
                Email = EmailBox.Text,
                Phone = PhoneBox.Text,
                Password = PasswordBox.Password
            };


            // Forsøger at gemme den nye bruger i UserStore.cs
            var success = UserStore.RegisterUser(newUser);

            // Hivs brugeren gemmes korrekt:
            if (success)
            {
                // Hvis brugeren blev oprettet, vises denne besked
                MessageBox.Show($"{newUser.Username} blev oprettet. Du kan nu logge ind");

                // Brugeren kommer tilbage til login siden
                NavigationService?.Navigate(new Login());
            }

            // Hvis brugeren ikke blev gemt korrekt:
            else
            {
                // Vises en fejlbesked
                MessageBox.Show("Brugeren blev ikke oprettet, prøv igen");
            }
        }

        //Hvis der trykkes på Annuller knappen:
        private void Cancel(object sender, RoutedEventArgs e)
        {
            // Kommer tilbage til login siden uden at gemme
            NavigationService?.Navigate(new Login());
        }
    }
}
