using System;
using System.Collections.ObjectModel;

namespace ActiviaAPP.Classes
{
    //////Kodet af Camilla
    public class ActivityClass
    {
        //Attributter for aktiviteten - ændret til properties for databinding
        public string ActivityTitle { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public int MaxParticipants { get; set; }
        public DateTime Date { get; set; }
        public int CurrentParticipantCount { get; set; }
        public ObservableCollection<string> RegisteredUsers { get; set; }

        //Constructor - initialiserer alle attributter med standardværdier
        public ActivityClass()
        {
            ActivityTitle = "";
            ActivityType = "";
            Description = "";
            MaxParticipants = 0;
            Date = DateTime.Now;
            CurrentParticipantCount = 0;
            RegisteredUsers = new ObservableCollection<string>();
        }

        //Metode til at tilmelde en bruger til aktiviteten
        public bool Register(string userID)
        {
            //Tjek om userID er tom
            if (userID == null || userID == "")
            {
                return false;
            }

            //Tjek om brugeren allerede er tilmeldt
            bool alreadyRegistered = false;
            for (int i = 0; i < RegisteredUsers.Count; i++)
            {
                if (RegisteredUsers[i] == userID)
                {
                    alreadyRegistered = true;
                    break;
                }
            }
            
            if (alreadyRegistered)
            {
                return false;
            }

            //Tjek om aktiviteten er fuld
            if (MaxParticipants > 0)
            {
                if (RegisteredUsers.Count >= MaxParticipants)
                {
                    return false;
                }
            }

            //Tilføj brugeren til listen
            RegisteredUsers.Add(userID);
            return true;
        }

        //Metode til at afmelde en bruger fra aktiviteten
        public bool Unregister(string userID)
        {
            //Tjek om userID er tom
            if (userID == null || userID == "")
            {
                return false;
            }

            //Fjern brugeren fra listen
            bool removed = RegisteredUsers.Remove(userID);
            return removed;
        }

        //Metode der returnerer aktivitetens information som en string
        public override string ToString()
        {
            string dateString = Date.ToString("dd-MM-yyyy");
            return ActivityTitle + " (" + dateString + ")";
        }
    }
}
