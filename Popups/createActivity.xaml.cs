using System;
using System.Windows;
using System.Windows.Controls;

namespace ActiviaAPP.Popups
{
    public partial class CreateActivity : Window
    {
        //Attributter som bruges til at sende data tilbage
        public string ActivityTitle;
        public string ActivityType;
        public DateTime ActivityDate;
        public string ActivityDescription;
        public int MaxParticipants;

        //Constructor
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
            if (ParticipantValue != null)
            {
                int value = (int)ParticipantSlider.Value;
                ParticipantValue.Text = value.ToString();
            }
        }

        //Metode til at oprette aktivitet
        private void createClick(object sender, RoutedEventArgs e)
        {
            //Tjek om titel er udfyldt
            string titleText = TitleBox.Text;
            if (titleText == null || titleText == "")
            {
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }
            
            //Tjek om type er valgt
            object selectedItemObj = TypeBox.SelectedItem;
            if (selectedItemObj == null)
            {
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }
            
            //Tjek om dato er valgt
            DateTime? selectedDateNullable = DateBox.SelectedDate;
            if (selectedDateNullable == null)
            {
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }
            
            //Tjek om beskrivelse er udfyldt
            string descriptionText = DescriptionBox.Text;
            if (descriptionText == null || descriptionText == "")
            {
                MessageBox.Show("Alle felter skal udfyldes");
                return;
            }

            //Hent værdier fra UI
            ActivityTitle = titleText;
            
            //Hent valgt type
            ComboBoxItem selectedItem = selectedItemObj as ComboBoxItem;
            if (selectedItem != null)
            {
                object content = selectedItem.Content;
                if (content != null)
                {
                    string contentString = content.ToString();
                    if (contentString != null)
                    {
                        ActivityType = contentString;
                    }
                }
            }
            
            if (ActivityType == null || ActivityType == "")
            {
                ActivityType = "";
            }
            
            //Hent valgt dato
            DateTime selectedDate = selectedDateNullable.Value;
            ActivityDate = selectedDate;
            
            ActivityDescription = descriptionText;
            MaxParticipants = (int)ParticipantSlider.Value;

            //Bekræft oprettelse
            MessageBox.Show("Aktivitet blev oprettet");
            this.DialogResult = true;
            this.Close();
        }

        //Metode til at annullere
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
