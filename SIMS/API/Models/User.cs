using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreApplication.Models
{
    public class User
    {
        public int BenutzerID { get; set; }
        public string BenutzerName { get; set; }
        public string Rolle { get; set; }
        public string Passwort { get; set; }
    }
}
