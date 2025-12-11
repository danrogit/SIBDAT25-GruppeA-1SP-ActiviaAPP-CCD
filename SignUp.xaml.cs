using ActiviaAPP.Classes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ActiviaAPP
{
    //Kodet af Casper
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
            //Hent værdier fra tekstfelterne
            string fullName = FullNameBox.Text;
            string username = UsernameBox.Text;
            string email = EmailBox.Text;
            string phone = PhoneBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            //Tjek om alle felter er udfyldt
            if (fullName == "" || username == "" || email == "" || phone == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("Udfyld alle felter");
                return;
            }

            //Tjek om adgangskoderne matcher
            if (password != confirmPassword)
            {
                MessageBox.Show("Adgangskoden matcher ikke");
                return;
            }

            //Tjek om brugernavnet allerede findes
            if (UserStore.UsernameExists(username))
            {
                MessageBox.Show("Brugernavnet er allerede taget");
                return;
            }

            //Opret ny bruger - RETTET: Brug Classes.User
            Classes.User newUser = new Classes.User();
            newUser.FullName = fullName;
            newUser.Username = username;
            newUser.Email = email;
            newUser.Phone = phone;
            newUser.Password = password;

            //Gem brugeren i UserStore
            bool success = UserStore.RegisterUser(newUser);

            //Hvis brugeren blev gemt korrekt
            if (success)
            {
                MessageBox.Show(username + " blev oprettet. Du kan nu logge ind");
                NavigationService.Navigate(new Login());
            }
            else
            {
                MessageBox.Show("Brugeren blev ikke oprettet, prøv igen");
            }
        }

        //Metode der annullerer oprettelsen
        private void Cancel(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
