using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ActiviaAPP.Classes;
using ActiviaAPP.Popups;
using Microsoft.Win32; 
using System.IO;
using System.Globalization;
                                                    
namespace ActiviaAPP
{
    public partial class Admin : Page
    {
        // Attributter til administratordata (kun eksempler)
        public string adminName = string.Empty;
        public string adminPassword = string.Empty;
        public string adminCompany = string.Empty;
      
        // Lokalt ObservableCollection (ikke brugt aktivt - vi binder til ActivityStore.activities direkte)
        private readonly ObservableCollection<ActivityClass> activities = new ObservableCollection<ActivityClass>();

        public Admin()
        {
            InitializeComponent();

            // Binder ActivityStore.activities til ListBox'en i Admin-siden, så UI viser de aktuelle aktiviteter.
            ActivityListBox.ItemsSource = ActivityStore.activities;

            // Binder UserStore.RegisteredUsers (medlemsliste) til UI-listen, så medlemmer vises og opdateres.
            userList.ItemsSource = UserStore.RegisteredUsers;
        }

        internal static ObservableCollection<ActivityClass> Userlists()
        {
            // Returnerer referencen til den delte aktivitetsliste fra ActivityStore.
            return ActivityStore.activities;
        }
            
        private void activityList(object sender, SelectionChangedEventArgs e)
        {   
            // Valgfrit: opdater UI ved selection-change
        }

        private void addActivity(object sender, RoutedEventArgs e)
        {
            // Åbn popup-vinduet for oprettelse af aktivitet
            var createActivity = new ActiviaAPP.Popups.CreateActivity();
            var result = createActivity.ShowDialog();

            // Hvis brugeren bekræfter oprettelse (DialogResult == true), bygg ActivityClass og tilføj til ActivityStore
            if (result == true)
            {
                var activity = new ActivityClass
                {
                    ActivityTitle = createActivity.ActivityTitle,
                    ActivityType = createActivity.ActivityType,
                    Date = createActivity.ActivityDate,
                    Description = createActivity.ActivityDescription,
                    MaxParticipants = createActivity.MaxParticipants,
                    CoverImagePath = createActivity.CoverImagePath
                };
                // Tilføj den nye aktivitet til den delte liste, som UI er bundet til.
               ActivityStore.activities.Add(activity);
            }                                     
        }

        private void removeActivity(object sender, RoutedEventArgs e)
        {
            // Fjern den valgte aktivitet fra listen efter bekræftelse
            var selected = ActivityListBox.SelectedItem as ActivityClass;
            if (selected == null)
            {
                MessageBox.Show("Vælg en aktivitet først");
                return;
            }

            var confirm = MessageBox.Show($"Er du sikker på du vil slette '{selected.ActivityTitle}'?", "Bekræft", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                ActivityStore.activities.Remove(selected);
                MessageBox.Show($"Aktiviteten '{selected.ActivityTitle}' er slettet");
            }
        }

        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering 
        // Åbn en filvælger og indlæs brugere via UserStore.LoadFromCsv
        private void adminSettings(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV filer (*.csv)|*.csv|Alle filer (*.*)|*.*",
                Title = "Vælg en CSV fil med brugere"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Brug den centraliserede LoadFromCsv metode i UserStore
                    UserStore.LoadFromCsv(openFileDialog.FileName);
                    MessageBox.Show($"Brugere er blevet indlæst fra filen!\n\nAntal brugere: {UserStore.RegisteredUsers.Count}",
                                    "Success",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Kunne ikke indlæse CSV filen:\n{ex.Message}",
                                    "Fejl",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
        }

        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
        //Metode til upload af aktiviteter fra CSV
        private void UploadActivities(object sender, RoutedEventArgs e)
        {
            //Opretter en OpenFileDialog til at vælge CSV fil
            var dlg = new OpenFileDialog
            {
                //Filtyper der kan vælges
                Filter = "CSV filer (*.csv)|*.csv|Alle filer (*.*)|*.*",

                //Overskrift for dialogen
                Title = "Vælg en CSV fil med aktiviteter"
            };

            //Hvis brugeren annullerer dialogen, afsluttes metoden
            if (dlg.ShowDialog() != true)
                return;

            //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
            try
            {
                //Læs fil som UTF-8
                var lines = File.ReadAllLines(dlg.FileName, Encoding.UTF8);

                //Tæller for antal tilføjede aktiviteter
                int added = 0;

                //Et loop der kører gennem hver linje i CSV filen
                foreach (var raw in lines)
                {
                    //Springer tomme linjer over
                    if (string.IsNullOrWhiteSpace(raw))
                        continue;

                    //Fjerne udnødvenige mellemrum
                    var line = raw.Trim();

                    //Fjerne omgivende parenteser hvis de er der
                    if (line.StartsWith("(") && line.EndsWith(")"))
                        line = line.Substring(1, line.Length - 2).Trim();

                    //Adskiller linjerne, der foretrækkes semikolon, ellers bruges komma
                    char delimiter = line.Contains(';') ? ';' : ',';

                    //Splitter linjen i felter, fjerner anførselstegn og mellemrum og tilføjer til et array
                    var fields = SplitLine(line, delimiter).Select(f => Unquote(f.Trim())).ToArray();

                    //Springer header-rækken over, hvis første felt ligner "title"
                    var first = fields.Length > 0 ? fields[0].ToLowerInvariant() : string.Empty;
                    if (first == "title" || first == "titel")
                        continue;

                    //Springer linjer uden felter over
                    if (fields.Length < 1)
                        continue;

                    // Titel felter: Title;Description;Date;MaxParticipants;[CurrentParticipants];[ImagePath]
                    string title = fields.Length > 0 ? fields[0] : string.Empty;
                    string description = fields.Length > 1 ? fields[1] : string.Empty;
                    string dateStr = fields.Length > 2 ? fields[2] : string.Empty;
                    string maxStr = fields.Length > 3 ? fields[3] : string.Empty;

                    //Kan køre to formater:
                    // 1) Title;Description;Date;MaxParticipants;ImagePath (5 kolonner)
                    // 2) Title;Description;Date;MaxParticipants;CurrentParticipants;ImagePath (6 kolonner)
                    string currentStr = fields.Length > 4 ? fields[4] : string.Empty;
                    string imgPath = fields.Length > 5 ? fields[5] : (fields.Length > 4 ? fields[4] : string.Empty);

                    // Spring linjer uden titel over
                    if (string.IsNullOrWhiteSpace(title))
                        continue;

                    // Parsing af dato (som før)
                    if (!DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)
                        && !DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        // Hvis dato er ugyldig, sættes dato til i dag
                        date = DateTime.Today;
                    }

                    // Parsing af max deltagere
                    int maxParticipants = 0;
                    if (!int.TryParse(maxStr, out maxParticipants))
                        maxParticipants = 0;

                    // Parsing af nuværende deltagere (valgfrit)
                    int currentParticipants = 0;
                    if (!string.IsNullOrWhiteSpace(currentStr))
                        int.TryParse(currentStr, out currentParticipants);

                    // Boolean der checker om aktiviteten allerede findes, baseret på titel og dato
                    bool exists = ActivityStore.activities.Any(a =>
                        string.Equals(a.ActivityTitle, title, StringComparison.OrdinalIgnoreCase) && a.Date.Date == date.Date);

                    // Hvis aktiviteten allerede findes, springes den over i filen
                    if (exists)
                        continue;

                    // Forsøger at løse billedstien
                    var resolvedImage = ResolveImagePath(imgPath);

                    // Opretter en ny aktivitet med de indlæste data
                    var activity = new ActivityClass
                    {
                        ActivityTitle = title,
                        Description = description,
                        Date = date,
                        MaxParticipants = maxParticipants,
                        CoverImagePath = resolvedImage
                    };


                    // Instead keep RegisteredUsers in sync so the read-only count reflects the intended value
                    if (activity.RegisteredUsers != null)
                    {
                        while (activity.RegisteredUsers.Count < Math.Max(0, currentParticipants))
                            activity.RegisteredUsers.Add($"imported-{Guid.NewGuid():N}");
                    }

                    // Tilføjer den nye aktivitet til den delte aktivitetsliste "ActivityStore.activities"
                    ActivityStore.activities.Add(activity);
                    added++;
                }

                //Succes besked med antal aktiviteter, når alle linjer er behandlet
                MessageBox.Show($"Aktiviteter importeret: {added}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
            //Fejlbesked hvis der opstår en undtagelse under indlæsningen af filen
            catch (Exception ex)
            {
                MessageBox.Show($"Kunne ikke indlæse aktivitets-CSV:\n{ex.Message}", "Fejl", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
        //Gør en tekststreng til en gyldig billedsti
        private static string ResolveImagePath(string path)
        {
            //Håndterer tomme stier
            if (string.IsNullOrWhiteSpace(path))
                return string.Empty;

            //Fjerner unødvendige mellemrum og anførselstegn
            var trimmed = path.Trim().Trim('"').Trim();

            // Normaliser skråstreger "\, /" til et format der passer til systemet        
            var normalized = trimmed.Replace('/', System.IO.Path.DirectorySeparatorChar).Replace('\\', System.IO.Path.DirectorySeparatorChar);
            
            //Tjekker om stien er absolut og om filen findes
            if (System.IO.Path.IsPathRooted(trimmed) && File.Exists(trimmed))

                //Returnerer den absolutte sti hvis filen findes
                return trimmed;

            // 2) Prøv relativt til appens basefolder
            //Tjekker om filen findes i applikationens base directory
            var appPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, normalized.TrimStart(System.IO.Path.DirectorySeparatorChar));
            if (File.Exists(appPath))
                return appPath;

            //Konverterer stien til en pack URI format
            var packPath = "pack://application:,,,/" + trimmed.Replace('\\', '/').TrimStart('/');
            return packPath;
        }

        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
        //SplitLine metode der læser linjerne i CSV-filen og opdeler felterne korrekt
        private static IEnumerable<string> SplitLine(string line, char delimiter)
        {
            //Bruger en StringBuilder til at bygge hvert felt
            var cur = new StringBuilder();
            bool inQuotes = false;

            //Går gennem hvert tegn i linjen
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];

                //Håndtering af anførselstegn og felter
                if (inQuotes)
                {

                    //Håndtering af anførselstegn inde i feltet
                    if (c == '"')
                    {
                        //
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            // Tilføj enkelt quote og spring næste over
                            cur.Append('"');
                            i++;
                        }
                        else
                        {
                            // Slut på quoted blok
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        // Normalt tegn inde i quotes
                        cur.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        // Start quoted blok
                        inQuotes = true;
                    }
                    else if (c == delimiter)
                    {
                        // Separator: yield nuværende felt
                        yield return cur.ToString();
                        cur.Clear();
                    }
                    else
                    {
                        // Almindeligt tegn
                        cur.Append(c);
                    }
                }
            }
            // Returner sidste felt
            yield return cur.ToString();
        }

        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
        // Fjern omgivelserende anførselstegn og af-escape dobbelte quotes.
        private static string Unquote(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (s.Length >= 2 && s[0] == '"' && s[^1] == '"')
                return s.Substring(1, s.Length - 2).Replace("\"\"", "\"");
            return s;
        }

        // Åbn detaljer for den valgte aktivitet i et popup-vindue
        private void OpenActivity(object sender, RoutedEventArgs e)
        {
            var selected = ActivityListBox.SelectedItem as ActivityClass;
            if (selected == null)
            {
                MessageBox.Show("Vælg en aktivitet først");
                return;
            }

            // Opret dialog og sæt ejer (så dialogcenterering/modale forhold virker korrekt)
            var dlg = new Popups.ActivityDetails(selected, "admin");
            dlg.Owner = Window.GetWindow(this);
            dlg.ShowDialog();
        }

        private void ActivityListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenActivity(sender, e);
        }

        private void logOut(object sender, RoutedEventArgs e)
        {
            // Naviger tilbage til login-siden
            NavigationService.Navigate(new Login());
        }

        private void userList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Placeholder hvis man vil håndtere valg af bruger
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Henter det valgte medlem fra userList
            var selectedUser = userList.SelectedItem as ActiviaAPP.Classes.User;

            // Tjekker om et medlem er valgt
            if (selectedUser == null)
            {
                // Besked hvis ingen medlem er valgt
                MessageBox.Show("Vælg et medlem først");
                return;
            }

            //Sletter det valgte medlem fra RegisteredUsers listen
            UserStore.RegisteredUsers.Remove(selectedUser);

            // Bekræftelsesbesked om sletning
            MessageBox.Show($"Brugeren '{selectedUser.FullName}' er blevet slettet");
        }
    }
}

