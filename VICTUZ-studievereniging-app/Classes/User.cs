using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.Classes
{
    public class User
    {
        public string? Id { get; set; }

        public string? Email { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Password { get; set; }

        public bool IsAdmin { get; set; } = false;

        //public List<Sugestion> SugestedEvents { get; set; }

        //public List<Events> JoinedEvents { get; set; }

        //public List<Events> HostedEvents { get; set; }

        //public List<Events> LikedEvents { get; set; }






    }
}
