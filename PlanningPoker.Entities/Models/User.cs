using System;
using System.Collections.Generic;

namespace PlanningPoker.Entities.Models
{
    public partial class User
    {
        public User()
        {
            Games = new HashSet<Game>();
        }

        public int Userid { get; set; }
        public string Name { get; set; } = null!;
        public string GameToken { get; set; } = null!;
        public int? SelectedCard { get; set; }
        public int? IsCardSelected { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}
