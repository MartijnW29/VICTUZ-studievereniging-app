using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.Classes
{
    public class Category
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public Image? Picture { get; set; }

        public List<Event>? Events { get; set; }

        public List<Suggestion>? Suggestions { get; set; }
    }
}
