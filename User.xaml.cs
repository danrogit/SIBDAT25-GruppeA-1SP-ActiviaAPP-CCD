using System;
using System.Windows;
using System.Windows.Controls;
using ActiviaAPP.Classes;
using ActiviaAPP.Popups;

namespace ActiviaAPP
{
    //////Kodet af Daniel og Camilla
    public partial class User : Page
    {
        //Attributter for brugeren - ændret til properties for databinding
        public string username { get; set; }
        public string userPassword { get; set; }
        public string userFullName { get; set; }
        public string userMail { get; set; }
        public int userPhone { get; set; }

        //Constructor
        public User()
        {
            InitializeComponent();

            //Initialiser attributter
            username = "";
            userPassword = "";
            userFullName = "";
            userMail = "";
            userPhone = 0;
            
            //Sæt DataContext så databinding virker
            DataContext = this;
            
            //Bind aktivitetslisten til UI
            UserListbox.ItemsSource = ActivityStore.activities;
        }

        //Metode til at logge ud
        private void logOut(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Tom metode
        }

        //Metode der kaldes ved dobbeltklik på aktivitet
        private void UserListbox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenSelectedActivityDetails();
        }

        //Metode der kaldes ved klik på "Åbn aktivitet" knap
        private void OpenActivity(object sender, RoutedEventArgs e)
        {
            OpenSelectedActivityDetails();
        }

        //Metode til at åbne aktivitets detaljer
        private void OpenSelectedActivityDetails()
        {
            //Hent den valgte aktivitet
            ActivityClass activity = UserListbox.SelectedItem as ActivityClass;

            //Tjek om der er valgt en aktivitet
            if (activity == null)
            {
                MessageBox.Show("Vælg en aktivitet først");
                return;
            }

            //Find bruger ID
            string userId = username;
            if (userId == "" || userId == null)
            {
                userId = userFullName;
            }
            if (userId == null)
            {
                userId = "";
            }

            //Åbn dialog vindue
            ActivityDetails dlg = new ActivityDetails(activity, userId);
            dlg.ShowDialog();
        }

        //Metode til at tilmelde sig en aktivitet
        private void SignUpActivity(object sender, RoutedEventArgs e) 
        {
            //Hent den valgte aktivitet
            ActivityClass activity = UserListbox.SelectedItem as ActivityClass;

            //Tjek om der er valgt en aktivitet
            if (activity == null)
            {
                MessageBox.Show("Vælg en aktivitet");
                return;
            }

            //Find bruger ID
            string userID = username;
            if (userID == "" || userID == null)
            {
                userID = userFullName;
            }

            //Tjek om bruger ID er tomt
            if (userID == "" || userID == null)
            {
                MessageBox.Show("Angiv dit brugernavn");
                return;
            }

            //Forsøg at tilmelde brugeren
            bool success = activity.Register(userID);

            //Hvis tilmelding lykkedes
            if (success)
            {
                MessageBox.Show("Du er tilmeldt: " + activity.ActivityTitle);
            }
            else
            {
                //Tjek hvorfor det fejlede
                
                //Tjek om brugeren allerede er tilmeldt
                bool alreadyRegistered = false;
                for (int i = 0; i < activity.RegisteredUsers.Count; i++)
                {
                    if (activity.RegisteredUsers[i] == userID)
                    {
                        alreadyRegistered = true;
                        break;
                    }
                }

                if (alreadyRegistered)
                {
                    MessageBox.Show("Du er allerede tilmeldt denne aktivitet");
                }
                //Tjek om aktiviteten er fuld
                else if (activity.MaxParticipants > 0 && activity.RegisteredUsers.Count >= activity.MaxParticipants)
                {
                    MessageBox.Show("Aktiviteten er fuld");
                }
                //Anden fejl
                else
                {
                    MessageBox.Show("Beklager, der skete en fejl");
                }
            }
        }
    }
}
