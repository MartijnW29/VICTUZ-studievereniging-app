using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VICTUZ_studievereniging_app.Classes
{
    public class Suggestion
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public Image? Picture { get; set; }

        public List<Category>? categories { get; set; }

        public User? Sugester { get; set; }
        public string? SugesterId { get; set; }

        public int Likes { get; set; } = 0;

        public List<User>? LikedBy { get; set; }
    }
}
