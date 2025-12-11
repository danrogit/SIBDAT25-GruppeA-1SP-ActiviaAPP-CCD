using System;
using System.Windows;
using System.Windows.Controls;

namespace ActiviaAPP.Popups
{
    public partial class CreateActivity : Window
    {
        //Attributter
        public string ActivityTitle;
        public string ActivityType;
        public DateTime ActivityDate;
        public string ActivityDescription;
        public int MaxParticipants;

        //Constructor der initialiserer komponenterne
        public CreateActivity()
        {
            InitializeComponent();
            
            //Initialiser attributter
            ActivityTitle = "";
            ActivityType = "";
            ActivityDate = DateTime.Now;
            ActivityDescription = "";
            MaxParticipants = 10;
        }

        //Metode til at opdatere slider værdi
        private void participantValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //Opdaterer tekstboksen med slider værdien
            if (ParticipantValue != null)
            {
                int value = (int)ParticipantSlider.Value;
                ParticipantValue.Text = value.ToString();
            }
        }

        //Metode til at oprette aktivitet
        private void createClick(object sender, RoutedEventArgs e)
        {
            //String der angiver titel ud fra tekstboksen
            string titleText = TitleBox.Text;

            //Hvis titel ikke er udfyldt:
            if (titleText == null || titleText == "")
            {
                //Vises en besked
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }

            //Angiver hvilken type af aktivitet der er valgt ud fra dropdown
            object selectedItemObj = TypeBox.SelectedItem;

            //Hvis ingen type er valgt:
            if (selectedItemObj == null)
            {
                //Vises en besked
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }

            //Angiver hvilken dato der er valgt ud fra DatePicker
            DateTime? selectedDateNullable = DateBox.SelectedDate;

            //Hvis ingen dato er valgt:
            if (selectedDateNullable == null)
            {
                //Vises en besked
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }

            //Angiver beskrivelsen ud fra tekstboksen
            string descriptionText = DescriptionBox.Text;

            //Hvis beskrivelse ikke er udfyldt:
            if (descriptionText == null || descriptionText == "")
            {
                //Vises en besked
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }
           
            //Hent værdier fra UI
            ActivityTitle = titleText;
            
            //Hent valgt type
            ComboBoxItem selectedItem = selectedItemObj as ComboBoxItem;

            //Sæt ActivityType baseret på valgt item
            if (selectedItem != null)
            {
                //Hent indholdet af det valgte item
                object content = selectedItem.Content;

                //Hvis ikke at content er null, konverter til string og sæt ActivityType
                if (content != null)
                {
                    //String til konvertering
                    string contentString = content.ToString();

                    //Hvis ikke null, sæt ActivityType
                    if (contentString != null)
                    {
                        ActivityType = contentString;
                    }
                }
            }

            //Sikrer at ActivityType ikke er null, ved at sætte til tom string
            if (ActivityType == null || ActivityType == "")
            {
                ActivityType = "";
            }
            
            //Hent valgt dato
            DateTime selectedDate = selectedDateNullable.Value;

            //Sætter ActivityDate
            ActivityDate = selectedDate;

            //Hent beskrivelse
            ActivityDescription = descriptionText;

            //Hent max deltagere fra slider
            MaxParticipants = (int)ParticipantSlider.Value;

            //Bekræftelse på oprettelse af aktivitet
            MessageBox.Show("Aktivitet blev oprettet");
            this.DialogResult = true;
            this.Close();
        }

        //Metode til at annullere opretelse af aktivitet
        private void cancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        //Event handler for titel ændring
        private void titleChange(object sender, TextChangedEventArgs e)
        {
            //Tom metode
        }
    }
}
