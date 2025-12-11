using ActiviaAPP.Classes;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ActiviaAPP
{
    public partial class Login : Page
    {
        //////Kodet af Daniel
        //Constructor
        public Login()
        {
            InitializeComponent();
        }

        //Metode der håndterer login
        private void logIn(object sender, RoutedEventArgs e)
        {
            //Hent brugernavn og password fra tekstfelterne
            string user = UsernameBox.Text;
            string pass = PasswordBox.Password;

            //Tjek om felterne er tomme
            if (user == "" || pass == "")
            {
                MessageBox.Show("Indtast brugernavn og adgangskode");
                return;
            }

            //Hardcoded admin login til demo
            if (user == "admin" && pass == "1")
            {
                NavigationService.Navigate(new Admin());
                return;
            }

            //Søg efter bruger i listen - RETTET: Brug Classes.User
            Classes.User foundUser = UserStore.FindUser(user, pass);

            //Hvis bruger findes, log ind
            if (foundUser != null)
            {
                //Opret User page (ikke Classes.User!)
                User userPage = new User();
                userPage.username = foundUser.Username;
                userPage.userFullName = foundUser.FullName;
                userPage.userMail = foundUser.Email;
                
                //Konverter phone til int
                int phoneNumber = 0;
                int.TryParse(foundUser.Phone, out phoneNumber);
                userPage.userPhone = phoneNumber;

                NavigationService.Navigate(userPage);
                return;
            }

            //Hardcoded bruger login til demo
            if (user == "user" && pass == "1")
            {
                NavigationService.Navigate(new User());
                return;
            }

            //Hvis login fejler
            MessageBox.Show("Forkert brugernavn eller adgangskode");
        }

        //Metode til at uploade brugere fra CSV fil
        private void UploadCsv(object sender, RoutedEventArgs e)
        {
            //Opret file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV filer (*.csv)|*.csv|Alle filer (*.*)|*.*";
            openFileDialog.Title = "Vælg en CSV fil med brugere";

            //Vis dialog
            bool? result = openFileDialog.ShowDialog();
            
            if (result == true)
            {
                try
                {
                    //Indlæs brugere fra filen
                    UserStore.LoadFromCsv(openFileDialog.FileName);

                    //Vis success besked
                    int count = UserStore.RegisteredUsers.Count;
                    MessageBox.Show("Brugere er blevet indlæst fra filen!\n\nAntal brugere: " + count, 
                                    "Success", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    //Vis fejlbesked
                    MessageBox.Show("Kunne ikke indlæse CSV filen:\n" + ex.Message, 
                                    "Fejl", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Error);
                }
            }
        }

        //Metode til at gå til signup side
        private void GoToSignUp(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUp());
        }
    }
}
