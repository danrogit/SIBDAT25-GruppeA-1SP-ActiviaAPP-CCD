using ActiviaAPP.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Login : Page
    {
        //Kodet af Daniel
        public Login()
        {
            InitializeComponent();
        }

        private void logIn(object sender, RoutedEventArgs e)
        {
            //Hent brugernavn og kode fra inputfelterne
            string user = UsernameBox.Text;
            string pass = PasswordBox.Password;

            // Tjekker om brugernavn eller adgangskode mangler
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                //Hvis et af felterne er tomme, vises denne besked og stoppes login processen
                MessageBox.Show("Indtast brugernavn og adgangskode");
                return;
            }

            //Hardcoded admin-login til brug af demo
            if (user == "admin" && pass == "1")
            {
                // Admin logges ind og navigeres til Admin-siden
                NavigationService?.Navigate(new Admin());
                return;
            }

            // Forsøger at finde en oprettet bruger i UserStore
            var foundUser = UserStore.FindUser(user, pass);

            // Hvis brugeren findes, navigeres brugeren til User-siden
            if (foundUser != null)
            {
                // Opretter User-side og sætter brugerens data før navigation
                var userPage = new User
                {
                    username = foundUser.Username,
                    userFullName = foundUser.FullName,
                    userMail = foundUser.Email,
                    userPhone = int.TryParse(foundUser.Phone, out int phone) ? phone : 0
                };

                // Navigerer til User-siden med brugerdata
                NavigationService?.Navigate(userPage);
                return;
            }

            //Hardcoded user-login til brug af demo
            else if (user == "user" && pass == "1")
            {
                // Går videre til user-siden
                NavigationService?.Navigate(new User());
                return;
            }

            //Hvis der ikke findes en bruger med de indtastede værdier:
            else
            {
                // Fejlbesked vises
                MessageBox.Show("Forkert brugernavn eller adgangskode");
            }
        }

        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
        // Håndter "Upload CSV"-knappens klik for at indlæse brugere fra en CSV-fil
        private void UploadCsv(object sender, RoutedEventArgs e)
        {
            // Opret og konfigurer OpenFileDialog
            var openFileDialog = new OpenFileDialog
            {
                //Udvalg af filtyper og dialogtitel
                Filter = "CSV filer (*.csv)|*.csv|Alle filer (*.*)|*.*",
                Title = "Vælg en CSV fil med brugere"
            };

            // Viser dialogen og tjekker om brugeren valgte en fil
            if (openFileDialog.ShowDialog() == true)
            {
                // Forsøger at indlæse brugere fra den valgte fil
                try
                {
                    // Indlæs brugere fra den valgte fil
                    UserStore.LoadFromCsv(openFileDialog.FileName);

                    // Viser succesbesked med antal indlæste brugere
                    MessageBox.Show($"Brugere er blevet indlæst fra filen!\n\nAntal brugere: {UserStore.RegisteredUsers.Count}", 
                                    "Success", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Information);
                }

                // Håndterer eventuelle fejl under indlæsning
                catch (Exception ex)
                {
                    // Viser fejl ved mislykket indlæsning
                    MessageBox.Show($"Kunne ikke indlæse CSV filen:\n{ex.Message}", 
                                    "Fejl", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Error);
                }
            }
        }

        // Naviger til SignUp-siden hvis brugeren vil oprette en ny bruger
        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SignUp());
        }
    }
}
