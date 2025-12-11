using System;
using System.Windows;
using System.Collections.Specialized;
using ActiviaAPP.Classes;

namespace ActiviaAPP.Popups
{
    public partial class ActivityDetails : Window
    {
        //Attributter
        private ActivityClass activity;
        private string userId;

        //Constructor
        public ActivityDetails(ActivityClass activity, string currentUserId)
        {
            InitializeComponent();

            //Gem aktivitet og bruger ID
            this.activity = activity;
            this.userId = currentUserId;
            
            //Hvis userId er null, sæt til tom string
            if (this.userId == null)
            {
                this.userId = "";
            }

            //Vis information
            Populate();

            //Lyt efter ændringer
            activity.RegisteredUsers.CollectionChanged += RegisteredUsers_CollectionChanged;
        }

        //Metode der kaldes når listen ændres - RETTET: Nullable sender parameter
        private void RegisteredUsers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Dispatcher.Invoke(Populate);
        }

        //Metode til at opdatere UI
        private void Populate()
        {
            //Vis titel
            TitleText.Text = activity.ActivityTitle;
            
            //Vis beskrivelse
            DescriptionText.Text = activity.Description;
            
            //Vis dato
            DateText.Text = activity.Date.ToString("dd-MM-yyyy");
            
            //Vis antal deltagere
            string participantsText = activity.RegisteredUsers.Count.ToString();
            participantsText = participantsText + "/";
            
            if (activity.MaxParticipants > 0)
            {
                participantsText = participantsText + activity.MaxParticipants.ToString();
            }
            else
            {
                participantsText = participantsText + "∞";
            }
            
            ParticipantsText.Text = participantsText;
            
            //Vis liste over tilmeldte
            if (activity.RegisteredUsers.Count == 0)
            {
                RegisteredList.Text = "Tilmeldte: Ingen";
            }
            else
            {
                string registeredText = "Tilmeldte: ";
                
                for (int i = 0; i < activity.RegisteredUsers.Count; i++)
                {
                    registeredText = registeredText + activity.RegisteredUsers[i];
                    
                    if (i < activity.RegisteredUsers.Count - 1)
                    {
                        registeredText = registeredText + ", ";
                    }
                }
                
                RegisteredList.Text = registeredText;
            }

            //Opdater tilmeld-knap
            bool isRegistered = false;
            for (int i = 0; i < activity.RegisteredUsers.Count; i++)
            {
                if (activity.RegisteredUsers[i] == userId)
                {
                    isRegistered = true;
                    break;
                }
            }

            if (isRegistered)
            {
                RegisterBtn.Content = "Tilmeldt";
                RegisterBtn.IsEnabled = false;
            }
            else
            {
                RegisterBtn.Content = "Tilmeld";
                
                //Tjek om aktiviteten er fuld
                if (activity.MaxParticipants == 0)
                {
                    RegisterBtn.IsEnabled = true;
                }
                else if (activity.RegisteredUsers.Count < activity.MaxParticipants)
                {
                    RegisterBtn.IsEnabled = true;
                }
                else
                {
                    RegisterBtn.IsEnabled = false;
                }
            }
        }

        //Metode til tilmelding
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            //Tjek om brugeren er tilmeldt
            bool isRegistered = false;
            for (int i = 0; i < activity.RegisteredUsers.Count; i++)
            {
                if (activity.RegisteredUsers[i] == userId)
                {
                    isRegistered = true;
                    break;
                }
            }

            if (isRegistered)
            {
                //Afmeld bruger
                bool removed = activity.Unregister(userId);
                if (removed)
                {
                    MessageBox.Show("Du er afmeldt aktiviteten");
                }
            }
            else
            {
                //Tjek om aktiviteten er fuld
                if (activity.MaxParticipants > 0 && activity.RegisteredUsers.Count >= activity.MaxParticipants)
                {
                    MessageBox.Show("Aktiviteten er desværre fuld");
                    Populate();
                    return;
                }

                //Tilmeld bruger
                bool ok = activity.Register(userId);
                
                if (ok)
                {
                    MessageBox.Show("Du er tilmeldt aktiviteten");
                }
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
