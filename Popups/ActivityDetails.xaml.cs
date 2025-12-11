using System;
using System.Windows;
using System.Collections.Specialized;
using ActiviaAPP.Classes;

namespace ActiviaAPP.Popups
{
    public partial class ActivityDetails : Window
    {

        /// <summary>
        /// Kodet af Daniel og Camilla
        /// </summary>

        //Attributter
        private ActivityClass activity;
        private string userId;


        //Konstructor
        public ActivityDetails(ActivityClass activity, string currentUserId)
        {
            InitializeComponent();

            //Tager den valgte aktivitet og bruger ID
            this.activity = activity;
            this.userId = currentUserId;

            //Gør at UserId ikke er null/tom
            if (this.userId == null)
            {
                this.userId = "";
            }

            //Henter og viser information om den valgte aktivitet i UI'et
            Populate();

            //Ser efter ændringer i listen af registrerede brugere og opdaterer UI'et hvis der sker ændringer
            activity.RegisteredUsers.CollectionChanged += RegisteredUsers_CollectionChanged;
        }


        //Metode der kaldes når RegisteredUsers listen ændres
        private void RegisteredUsers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Populate();
        }


        //Metode til at opdatere UI
        private void Populate()
        {
            //Viser titel
            TitleText.Text = activity.ActivityTitle;
            
            //Viser beskrivelse
            DescriptionText.Text = activity.Description;
            
            //Viser dato
            DateText.Text = activity.Date.ToString("dd-MM-yyyy");
            
            //Viser antal deltagere
            string participantsText = activity.RegisteredUsers.Count.ToString();
            participantsText = participantsText + "/";

            //Hvis MaxParticipants er større end 0, vises det maksimale antal deltagere
            if (activity.MaxParticipants > 0)
            {
                //Viser maks antal deltagere
                participantsText = participantsText + activity.MaxParticipants.ToString();
            }

            //Hvis MaxParticipants er 0, er der ingen deltagerbegrænsning, og den sættes til uendelig
            else
            {
                //Viser uendelig deltagerantal
                participantsText = participantsText + "∞";
            }

            //Opdaterer tekstfeltet
            ParticipantsText.Text = participantsText;

            //Hvis ingen er tilmeldte til aktiviteten, vises "Ingen"
            if (activity.RegisteredUsers.Count == 0)
            {
                //Ingen er tilmeldt
                RegisteredList.Text = "Tilmeldte: Ingen";
            }

            //Hvis der er tilmeldte, vises deres ID'er
            else
            {
                //String der angiver de tilmeldte brugere
                string registeredText = "Tilmeldte: ";

                //Loop der kører igennem alle tilmeldte brugere og tilføjer deres ID'er til teksten på UI'et
                for (int i = 0; i < activity.RegisteredUsers.Count; i++)
                {                   
                    registeredText = registeredText + activity.RegisteredUsers[i];

                    //Tilføj komma mellem ID'erne, undtagen efter det sidste
                    if (i < activity.RegisteredUsers.Count - 1)
                    {
                        registeredText = registeredText + ", ";
                    }
                }

                //Opdaterer tekstfeltet
                RegisteredList.Text = registeredText;
            }

            //Bool der angiver om brugeren er tilmeldt
            bool isRegistered = false;

            //For-loop der tjekker om den nuværende bruger er tilmeldt aktiviteten
            for (int i = 0; i < activity.RegisteredUsers.Count; i++)
            {
                //If sætning, hvis bruger ID'et findes i listen af tilmeldte brugere
                if (activity.RegisteredUsers[i] == userId)
                {
                    isRegistered = true;
                    break;
                }
            }

            //If sætning der opdaterer knappen baseret på om brugeren er tilmeldt eller ej
            if (isRegistered)
            {
                RegisterBtn.Content = "Tilmeldt";
                RegisterBtn.IsEnabled = false;
            }

            //Hvis ikke brugeren er tilmeldt
            else
            {
                //Opdater knappen til at vise "Tilmeld"
                RegisterBtn.Content = "Tilmeld";

                //Aktivitet er uden deltagerbegrænsning
                if (activity.MaxParticipants == 0)
                {
                    //Det er muligt at tilmelde sig
                    RegisterBtn.IsEnabled = true;
                }

                //Hvis aktiviteten har en deltagerbegrænsning, men der stadig er plads
                else if (activity.RegisteredUsers.Count < activity.MaxParticipants)
                {
                    //Det er muligt at tilmelde sig
                    RegisterBtn.IsEnabled = true;
                }

                //Hvis aktiviteten er fuld
                else
                {
                    //Det er ikke muligt at tilmelde sig
                    RegisterBtn.IsEnabled = false;
                }
            }
        }


        //Metode til tilmelding
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            //Tjek om brugeren er tilmeldt
            bool isRegistered = false;

            //For loop der tjekker om den nuværende bruger er tilmeldt aktiviteten
            for (int i = 0; i < activity.RegisteredUsers.Count; i++)
            {
                //If sætning, hvis bruger ID'et findes i listen af tilmeldte brugere
                if (activity.RegisteredUsers[i] == userId)
                {
                    isRegistered = true;
                    break;
                }
            }

            //Hvis brugeren er tilmeldt
            if (isRegistered)
            {
                //Afmeld bruger
                bool removed = activity.Unregister(userId);

                //Viser besked om afmelding
                if (removed)
                {
                    MessageBox.Show("Du er afmeldt aktiviteten");
                }


            } //Hvis brugeren ikke er tilmeldt
            else
            {
                //Hvis aktiviteten er fuld, vises besked og der afbrydes
                if (activity.MaxParticipants > 0 && activity.RegisteredUsers.Count >= activity.MaxParticipants)
                {
                    MessageBox.Show("Aktiviteten er desværre fuld");
                    Populate();
                    return;
                }

                //Tilmeld bruger
                bool ok = activity.Register(userId);

                //Viser besked om tilmelding
                if (ok)
                {
                    MessageBox.Show("Du er tilmeldt aktiviteten");
                }

                //Hvis tilmelding fejler
                else
                {
                    MessageBox.Show("Kunne ikke tilmelde - der opstod en fejl");
                }
            }

            //Opdater UI
            Populate();
        }

        //Metode til at lukke vinduet
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
