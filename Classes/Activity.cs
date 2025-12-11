using System;
using System.Collections.ObjectModel;

namespace ActiviaAPP.Classes
{
    /// <summary>
    /// Kodet af alle
    /// </summary>

    public class ActivityClass
    {
        //Attributter for aktiviteten
        public string ActivityTitle { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public int MaxParticipants { get; set; }
        public DateTime Date { get; set; }
        public int CurrentParticipantCount { get; set; }

        //Liste over registrerede brugere
        public ObservableCollection<string> RegisteredUsers { get; set; }

        //Constructor der initialiserer alle attributter med standardværdier
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
            //Hvis userID er tom, returneres false, og brugeren tilmeldes ikke
            if (userID == null || userID == "")
            {
                return false;
            }

            //Bool til tjek om brugeren allerede er tilmeldt
            bool alreadyRegistered = false;

            //Loop gennem listen af registrerede brugere og tjekker om userID allerede findes
            for (int i = 0; i < RegisteredUsers.Count; i++)
            {
                //Hvis userID findes, sættes alreadyRegistered til true og loopet brydes
                if (RegisteredUsers[i] == userID)
                {
                    alreadyRegistered = true;
                    break;
                }
            }

            //Hvis brugeren allerede er tilmeldt, returneres false
            if (alreadyRegistered)
            {
                return false;
            }

            //Tjek om aktiviteten er fuld
            if (MaxParticipants > 0)
            {
                //Hvis antallet af registrerede brugere er lig med eller større end max deltagere, returneres false
                if (RegisteredUsers.Count >= MaxParticipants)
                {
                    return false;
                }
            }

            //Tilføjer brugeren til listen
            RegisteredUsers.Add(userID);
            return true;
        }

        //Metode til at afmelde en bruger fra aktiviteten
        public bool Unregister(string userID)
        {
            //Tjekker om userID er tom
            if (userID == null || userID == "")
            {
                return false;
            }

            //Fjerner brugeren fra listen
            bool removed = RegisteredUsers.Remove(userID);
            return removed;
        }

        //Metode der returnerer aktivitetens information som en string
        public override string ToString()
        {
            //Formaterer datoen som dd-MM-yyyy
            string dateString = Date.ToString("dd-MM-yyyy");

            //Returnerer aktivitetens titel og dato
            return ActivityTitle + " (" + dateString + ")";
        }
    }
}
