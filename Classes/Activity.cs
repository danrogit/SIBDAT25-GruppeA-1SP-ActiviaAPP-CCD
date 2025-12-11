using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace ActiviaAPP.Classes
{
    //Kodet af Camilla
    public class ActivityClass
    {
        // Public attributter som bruges i UI og når aktiviteter oprettes
        public string ActivityTitle { get; set; } = string.Empty; 
        public string ActivityType { get; set; } = string.Empty;  
        public string Description { get; set; } = string.Empty;

        // Max antal deltagere (0 = ingen begrænsning)
        public int MaxParticipants { get; set; }                  
        public DateTime Date { get; set; }                        
        public string CoverImagePath { get; set; } = string.Empty;
        
        //Get set for hvor mange der er tilmeldt aktiviteten
        public int CurrentParticipantCount { get; set; }
       
        //Liste over brugere der er tilmeldt aktiviteten, ObservableCollection gør det muligt for UI at opdatere sig selv automatisk
        public ObservableCollection<string> RegisteredUsers { get; } = new ObservableCollection<string>();        

        //Public metode til at tilmelde en bruger til aktiviteten
        public bool Register(string userID)
        {
            //Hvis userID er tomt eller null, returneres false
            if (string.IsNullOrWhiteSpace(userID))
                return false;

            //Hvis brugeren allerede findes i listen over tilmeldte brugere, returneres false
            if (RegisteredUsers.Contains(userID))
                return false;

            //Hvis alle pladser er optaget på aktiviteten, returneres false
            if (MaxParticipants > 0 && RegisteredUsers.Count >= MaxParticipants)
                return false;

            //Tilføjer brugeren til listen over tilmeldte
            RegisteredUsers.Add(userID);
            return true;
        }

        //Public metode til at afmelde en bruger fra aktiviteten
        public bool Unregister(string userID)
        {
            //Hvis userID er tomt eller null, returneres false
            if (string.IsNullOrWhiteSpace(userID))
                return false;

            //Hvis brugeren findes, fjernes den fra listen over tilmeldte
            return RegisteredUsers.Remove(userID);
        }

        //Override to ToString, der viser titel og dato i formatet dd-MM-yyyy
        public override string ToString()
        {
            //Viser titel og dato i formatet dd-MM-yyyy
            return $"{ActivityTitle} ({Date:dd-MM-yyyy})";
        }
    }
}
