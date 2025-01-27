using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;// nodig zodat er niet infinite loops komen

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

        [JsonIgnore]// nodig zodat er niet infinite loops komen
        public List<Suggestion>? SugestedEvents { get; set; }

        [JsonIgnore]// nodig zodat er niet infinite loops komen
        public List<Event>? JoinedEvents { get; set; }

        [JsonIgnore]// nodig zodat er niet infinite loops komen
        public List<Event>? HostedEvents { get; set; }

        public List<Event>? LikedEvents { get; set; }






    }
}
