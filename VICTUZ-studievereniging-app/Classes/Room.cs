using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.Classes
{
    public class Room
    {
        public string? Id { get; set; }

        public string? RoomNumber { get; set; }

        public int Capacity { get; set; }

        public List<Event>? Events { get; set; }
    }
}
