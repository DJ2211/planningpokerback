using System;
using System.Collections.Generic;

namespace PlanningPoker.Entities.Models
{
    public partial class Game
    {
        public int Gameid { get; set; }
        public int? Userid { get; set; }

        public virtual User? User { get; set; }
    }
}
