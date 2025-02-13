﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.Classes
{
    public class Event
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public Image? Picture { get; set; }

        public List<Category>? Categories { get; set; } = new List<Category>();

        public int JoinLimit { get; set; }

        public Room? Room { get; set; }
        public string? RoomId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public List<User>? Registered { get; set; } = new List<User>();

        public List<User>? Attended { get; set; } = new List<User>();

        public List<User>? Hosts { get; set; } = new List<User>();




        public bool IsUserNotHost { get; set; }
    }
}
