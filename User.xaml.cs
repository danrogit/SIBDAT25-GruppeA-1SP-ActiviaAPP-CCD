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
        //Attributter
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
            
            //Binder aktivitetslisten til UI
            UserListbox.ItemsSource = ActivityStore.activities;
        }

        //Metode til at logge ud
        private void logOut(object sender, RoutedEventArgs e)
        {
            //Navigerer tilbage til login siden
            NavigationService.Navigate(new Login());
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Tom metode
        }

        //Metode til at åbne aktivitets detaljer
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
            //Henter den valgte aktivitet fra listen
            ActivityClass activity = UserListbox.SelectedItem as ActivityClass;

            //Hvis ingen aktivitet er valgt:
            if (activity == null)
            {
                //Vises fejlmeddelelse
                MessageBox.Show("Vælg en aktivitet først");
                return;
            }

            //String user ID
            string userId = username;

            //Hvis brugernavn er tomt, brug fulde navn
            if (userId == "" || userId == null)
            {
                userId = userFullName;
            }

            //Hvis bruger ID stadig er tomt, sæt til tom streng
            if (userId == null)
            {
                userId = "";
            }

            //Åbner aktivitetsdetaljer dialogen
            ActivityDetails dlg = new ActivityDetails(activity, userId);
            dlg.ShowDialog();
        }

        //Metode til at tilmelde sig en aktivitet
        private void SignUpActivity(object sender, RoutedEventArgs e) 
        {
            //Henter den valgte aktivitet fra listen
            ActivityClass activity = UserListbox.SelectedItem as ActivityClass;

            //Hvis ikke der er valgt en aktivitet:
            if (activity == null)
            {
                //Vis fejlmeddelelse
                MessageBox.Show("Vælg en aktivitet");
                return;
            }

            //String bruger ID
            string userID = username;

            //Hvis brugernavn er tomt, brug fulde navn
            if (userID == "" || userID == null)
            {
                userID = userFullName;
            }

            //Hvis bruger ID er tomt:
            if (userID == "" || userID == null)
            {
                //Vises fejlmeddelelse
                MessageBox.Show("Angiv dit brugernavn");
                return;
            }

            //Bool til at tjekke om tilmelding lykkedes
            bool success = activity.Register(userID);

            //Hvis tilmelding lykkedes:
            if (success)
            {
                //Vis bekræftelsesmeddelelse
                MessageBox.Show("Du er tilmeldt: " + activity.ActivityTitle);
            }

            //Hvis tilmelding fejlede:
            else
            {
                //Bool til at tjekke om bruger allerede er registreret til aktiviteten
                bool alreadyRegistered = false;

                //Loop gennem registrerede brugere
                for (int i = 0; i < activity.RegisteredUsers.Count; i++)
                {
                    //Hvis bruger allerede er registreret:
                    if (activity.RegisteredUsers[i] == userID)
                    {
                        //Bruger er allerede registreret
                        alreadyRegistered = true;
                        break;
                    }
                }

                //Hvis brugeren allerede er registreret:
                if (alreadyRegistered)
                {
                    //Vises fejlmeddelelse
                    MessageBox.Show("Du er allerede tilmeldt denne aktivitet");
                }

                //Hvis aktiviteten er fuld:
                else if (activity.MaxParticipants > 0 && activity.RegisteredUsers.Count >= activity.MaxParticipants)
                {
                    //Vises fejlmeddelelse
                    MessageBox.Show("Aktiviteten er fuld");
                }

                //Ved anden fejl
                else
                {
                    MessageBox.Show("Beklager, der skete en fejl");
                }
            }
        }
    }
}
