using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ActiviaAPP.Classes;
using ActiviaAPP.Popups;
using Microsoft.Win32;
using System.IO;
using System.Text;

namespace ActiviaAPP
{
    public partial class Admin : Page
    {
        //Attributter til admin
        public string adminName;
        public string adminPassword;
        public string adminCompany;

        //Constructor
        public Admin()
        {
            InitializeComponent();

            //Initialiser attributter
            adminName = "";
            adminPassword = "";
            adminCompany = "";

            //Bind lister til UI
            ActivityListBox.ItemsSource = ActivityStore.activities;
            userList.ItemsSource = UserStore.RegisteredUsers;
        }

        private void activityList(object sender, SelectionChangedEventArgs e)
        {
            //Tom metode - kan bruges senere
        }

        //Metode til at tilføje en aktivitet
        private void addActivity(object sender, RoutedEventArgs e)
        {
            //Åbn popup vindue
            CreateActivity createActivity = new CreateActivity();
            bool? result = createActivity.ShowDialog();

            //Hvis brugeren trykkede OK
            if (result == true)
            {
                //Opret ny aktivitet
                ActivityClass activity = new ActivityClass();
                activity.ActivityTitle = createActivity.ActivityTitle;
                activity.ActivityType = createActivity.ActivityType;
                activity.Date = createActivity.ActivityDate;
                activity.Description = createActivity.ActivityDescription;
                activity.MaxParticipants = createActivity.MaxParticipants;
                
                //Tilføj til listen
                ActivityStore.activities.Add(activity);
            }
        }

        //Metode til at fjerne en aktivitet
        private void removeActivity(object sender, RoutedEventArgs e)
        {
            //Hent den valgte aktivitet - RETTET: Explicit null check
            object selectedObj = ActivityListBox.SelectedItem;
            ActivityClass selected = selectedObj as ActivityClass;
            
            if (selected == null)
            {
                MessageBox.Show("Vælg en aktivitet først");
                return;
            }

            //Spørg om brugeren er sikker
            MessageBoxResult confirm = MessageBox.Show("Er du sikker på du vil slette '" + selected.ActivityTitle + "'?", 
                                                       "Bekræft", 
                                                       MessageBoxButton.YesNo, 
                                                       MessageBoxImage.Question);
            
            if (confirm == MessageBoxResult.Yes)
            {
                ActivityStore.activities.Remove(selected);
                MessageBox.Show("Aktiviteten '" + selected.ActivityTitle + "' er slettet");
            }
        }
       
        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering 

        //Metode til at indlæse brugere fra CSV
        private void adminSettings(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV filer (*.csv)|*.csv|Alle filer (*.*)|*.*";
            openFileDialog.Title = "Vælg en CSV fil med brugere";

            bool? result = openFileDialog.ShowDialog();
            
            if (result == true)
            {
                try
                {
                    UserStore.LoadFromCsv(openFileDialog.FileName);
                    
                    int userCount = UserStore.RegisteredUsers.Count;
                    MessageBox.Show("Brugere er blevet indlæst fra filen!\n\nAntal brugere: " + userCount,
                                    "Success",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kunne ikke indlæse CSV filen:\n" + ex.Message,
                                    "Fejl",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }
        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering 

        //Metode til at uploade aktiviteter fra CSV
        private void UploadActivities(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "CSV filer (*.csv)|*.csv|Alle filer (*.*)|*.*";
            dlg.Title = "Vælg en CSV fil med aktiviteter";

            bool? result = dlg.ShowDialog();
            
            if (result != true)
            {
                return;
            }
            //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering 

            try
            {
                //Læs alle linjer fra filen
                string[] lines = File.ReadAllLines(dlg.FileName, Encoding.UTF8);

                //Tæller for tilføjede aktiviteter
                int added = 0;

                //Loop gennem hver linje
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    
                    //Spring tomme linjer over
                    if (line == null || line == "")
                    {
                        continue;
                    }

                    //Fjern mellemrum
                    line = line.Trim();

                    //Split linjen ved semikolon eller komma
                    char delimiter = ';';
                    if (line.Contains(";"))
                    {
                        delimiter = ';';
                    }
                    else
                    {
                        delimiter = ',';
                    }
                    
                    string[] fields = line.Split(delimiter);

                    //Spring header over
                    if (fields.Length > 0)
                    {
                        string first = fields[0].Trim().ToLower();
                        if (first == "title" || first == "titel")
                        {
                            continue;
                        }
                    }

                    //Skal have mindst 1 felt
                    if (fields.Length < 1)
                    {
                        continue;
                    }

                    //Læs felter: Title;Description;Date;MaxParticipants;CurrentParticipants
                    string title = "";
                    string description = "";
                    string dateStr = "";
                    string maxStr = "";
                    string currentStr = "";

                    if (fields.Length > 0)
                    {
                        title = fields[0].Trim();
                    }
                    if (fields.Length > 1)
                    {
                        description = fields[1].Trim();
                    }
                    if (fields.Length > 2)
                    {
                        dateStr = fields[2].Trim();
                    }
                    if (fields.Length > 3)
                    {
                        maxStr = fields[3].Trim();
                    }
                    if (fields.Length > 4)
                    {
                        currentStr = fields[4].Trim();
                    }

                    //Spring over hvis titel er tom
                    if (title == null || title == "")
                    {
                        continue;
                    }

                    //Parse dato
                    DateTime date = DateTime.Today;
                    DateTime.TryParse(dateStr, out date);

                    //Parse max deltagere
                    int maxParticipants = 0;
                    int.TryParse(maxStr, out maxParticipants);

                    //Parse nuværende deltagere
                    int currentParticipants = 0;
                    if (currentStr != null && currentStr != "")
                    {
                        int.TryParse(currentStr, out currentParticipants);
                    }

                    //Tjek om aktiviteten allerede findes
                    bool exists = false;
                    for (int j = 0; j < ActivityStore.activities.Count; j++)
                    {
                        ActivityClass existingActivity = ActivityStore.activities[j];
                        
                        if (existingActivity.ActivityTitle.ToLower() == title.ToLower() && 
                            existingActivity.Date.Date == date.Date)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (exists)
                    {
                        continue;
                    }

                    //Opret ny aktivitet
                    ActivityClass activity = new ActivityClass();
                    activity.ActivityTitle = title;
                    activity.Description = description;
                    activity.Date = date;
                    activity.MaxParticipants = maxParticipants;

                    //Tilføj dummy brugere
                    for (int k = 0; k < currentParticipants; k++)
                    {
                        string dummyUser = "imported-" + Guid.NewGuid().ToString();
                        activity.RegisteredUsers.Add(dummyUser);
                    }

                    //Tilføj aktiviteten
                    ActivityStore.activities.Add(activity);
                    added++;
                }

                //Vis success besked
                MessageBox.Show("Aktiviteter importeret: " + added, 
                                "Success", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kunne ikke indlæse aktivitets-CSV:\n" + ex.Message, 
                                "Fejl", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
        }

        //Metode til at åbne aktivitetsdetaljer
        private void OpenActivity(object sender, RoutedEventArgs e)
        {
            //RETTET: Explicit null check
            object selectedObj = ActivityListBox.SelectedItem;
            ActivityClass selected = selectedObj as ActivityClass;
            
            if (selected == null)
            {
                MessageBox.Show("Vælg en aktivitet først");
                return;
            }

            //Opret dialog
            ActivityDetails dlg = new ActivityDetails(selected, "admin");
            
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                dlg.Owner = parentWindow;
            }
            
            dlg.ShowDialog();
        }

        private void ActivityListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenActivity(sender, e);
        }

        private void logOut(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Tom metode
        }

        //Metode til at slette en bruger
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Hent valgt bruger - RETTET: Explicit null check
            object selectedObj = userList.SelectedItem;
            Classes.User selectedUser = selectedObj as Classes.User;

            if (selectedUser == null)
            {
                MessageBox.Show("Vælg et medlem først");
                return;
            }

            //Slet brugeren
            UserStore.RegisteredUsers.Remove(selectedUser);

            MessageBox.Show("Brugeren '" + selectedUser.FullName + "' er blevet slettet");
        }
    }
}

