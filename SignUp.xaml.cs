using ActiviaAPP.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ActiviaAPP
{
    /// <summary>
    /// Kodet af Casper
    /// </summary>
    public partial class SignUp : Page
    {
        //Constructor
        public SignUp()
        {
            InitializeComponent();
        }

        //Metode der opretter en ny bruger
        private void CreateUser(object sender, RoutedEventArgs e)
        {
            //Henter værdierne fra tekstfelterne
            string fullName = FullNameBox.Text;
            string username = UsernameBox.Text;
            string email = EmailBox.Text;
            string phone = PhoneBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            //Hvis ikke alle felter er udfyldt:
            if (fullName == "" || username == "" || email == "" || phone == "" || password == "" || confirmPassword == "")
            {
                //Vises fejlmeddelelsen
                MessageBox.Show("Udfyld alle felter");
                return;
            }

            //Hvis ikke adgangskoderne matcher:
            if (password != confirmPassword)
            {
                //Vises fejlmeddelelsen
                MessageBox.Show("Adgangskoden matcher ikke");
                return;
            }

            //Hvis brugernavnet allerede findes:
            if (UserStore.UsernameExists(username))
            {
                //Vises fejlmeddelelsen
                MessageBox.Show("Brugernavnet er allerede taget");
                return;
            }

            //Oprettelse af ny bruger
            Classes.User newUser = new Classes.User();
            newUser.FullName = fullName;
            newUser.Username = username;
            newUser.Email = email;
            newUser.Phone = phone;
            newUser.Password = password;

            //Brugeren gemmes i UserStore
            bool success = UserStore.RegisterUser(newUser);

            //Hvis brugeren blev gemt korrekt:
            if (success)
            {
                //Vises bekræftelsesmeddelelsen
                MessageBox.Show(username + " blev oprettet. Du kan nu logge ind");
                NavigationService.Navigate(new Login());
            }

            //Hvis brugeren ikke blev gemt korrekt:
            else
            {
                //Vises fejlmeddelelsen
                MessageBox.Show("Brugeren blev ikke oprettet, prøv igen");
            }
        }

        //Metode der annullerer oprettelsen
        private void Cancel(object sender, RoutedEventArgs e)
        {
            //Navigerer tilbage til login siden
            NavigationService.Navigate(new Login());
        }
    }
}
