using System;
using System.Collections.ObjectModel;
using System.IO;

namespace ActiviaAPP.Classes
{
    /// <summary>
    /// Kodet af alle
    /// </summary>

    //Klasse til håndtering af brugere
    public class UserStore
    {
        //Liste over alle registrerede brugere
        public static ObservableCollection<User> RegisteredUsers = new ObservableCollection<User>();

        //Metode til at finde en bruger baseret på brugernavn og password, til login
        public static User FindUser(string username, string password)
        {
            //For loop gennem alle brugere
            for (int i = 0; i < RegisteredUsers.Count; i++)
            {
                User user = RegisteredUsers[i];

                //Sammenlign brugernavn og password, der ignoreres store og små bogstaver i brugernavn
                string usernameLower = user.Username.ToLower();
                string inputUsernameLower = username.ToLower();

                //Hvis både brugernavn og password matcher, returner brugeren
                if (usernameLower == inputUsernameLower && user.Password == password)
                {
                    return user;
                }
            }

            //Hvis ingen bruger blev fundet, returneres null
            return null;
        }

        //Metode til at tjekke om et brugernavn allerede eksisterer
        public static bool UsernameExists(string username)
        {
            //Loop gennem alle brugere
            for (int i = 0; i < RegisteredUsers.Count; i++)
            {
                //Sammenlign brugernavne, ignorer store og små bogstaver
                string existingUsername = RegisteredUsers[i].Username.ToLower();
                string inputUsername = username.ToLower();

                //Hvis brugernavnet findes, returneres true
                if (existingUsername == inputUsername)
                {
                    return true;
                }
            }

            //Hvis brugernavnet ikke findes, returneres false
            return false;
        }

        //Metode til at registrere en ny bruger
        public static bool RegisterUser(User user)
        {
            //Hvis brugernavn allerede eksiterer, returneres false
            if (UsernameExists(user.Username))
            {
                return false;
            }

            //Tilføj brugeren til listen
            RegisteredUsers.Add(user);
            return true;
        }

        //Denne kode er genereret ved hjælp af AI-værktøj, da vi har begrænset erfaring med denne type implementering
        //Metode til at indlæse brugere fra en CSV fil
        public static void LoadFromCsv(string filePath)
        {
            //Tjek om filen eksisterer
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("CSV fil ikke fundet");
            }

            //Læs alle linjer fra filen
            string[] lines = File.ReadAllLines(filePath);

            //Ryd listen først
            RegisteredUsers.Clear();

            //Loop gennem hver linje
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                //Spring tomme linjer over
                if (line == "")
                {
                    continue;
                }

                //Split linjen ved semikolon
                string[] parts = line.Split(';');

                //Skal have mindst 5 dele
                if (parts.Length < 5)
                {
                    continue;
                }

                //Tjek om det er header-linjen
                string firstPart = parts[0].Trim().ToLower();
                if (firstPart == "username" || firstPart == "brugernavn" || firstPart == "fullname" || firstPart == "navn")
                {
                    continue;
                }

                //Hent data fra CSV: FullName;Username;Password;Email;Phone
                string fullname = parts[0].Trim();
                string username = parts[1].Trim();
                string password = parts[2].Trim();
                string email = parts[3].Trim();
                string phone = parts[4].Trim();

                //Spring over hvis username er tom
                if (username == "")
                {
                    continue;
                }

                //Opret ny bruger
                User newUser = new User();
                newUser.Username = username;
                newUser.Password = password;
                newUser.FullName = fullname;
                newUser.Email = email;
                newUser.Phone = phone;

                //Tilføj kun hvis brugernavnet ikke allerede eksisterer
                if (!UsernameExists(newUser.Username))
                {
                    RegisteredUsers.Add(newUser);
                }
            }
        }
    }
}