using ActiviaAPP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiviaAPP.Classes
{
    //Kodet af alle
    public class User
    {
        //Brugerens oplysninger, som bliver sat ved oprettelse af bruger, er tom som standard
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        
        public override string ToString()
        {
            //Returnerer brugerens fulde navn og brugernavn i en læsbar form
            return $"{FullName} ({Username})";
        }

    }
}
