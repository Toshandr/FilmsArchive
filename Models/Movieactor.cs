using System;
using System.Collections.Generic;

namespace FilmsArchive.Models
{
    public partial class Movieactor
    {
        public int Movieid { get; set; }
        public int Actorid { get; set; }
        public string? Role { get; set; }

        public virtual Actor Actor { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
}
