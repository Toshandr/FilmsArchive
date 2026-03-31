using System;
using System.Collections.Generic;

namespace FilmsArchive.Models  //ef scaffold сгенерировал мне такие модели из бд вместе с хештаблицами, прошу не бить за них что не такие как в тз
{
    public partial class Actor
    {
        public Actor()
        {
            Movieactors = new HashSet<Movieactor>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Birthyear { get; set; }

        public virtual ICollection<Movieactor> Movieactors { get; set; }
    }
}
