using System;
using System.Collections.Generic;

namespace FilmsArchive.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Movieactors = new HashSet<Movieactor>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Movieactor> Movieactors { get; set; }
    }
}
