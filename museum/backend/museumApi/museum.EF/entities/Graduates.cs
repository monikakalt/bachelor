using System;
using System.Collections.Generic;

namespace museumApi.EF.entities
{
    public partial class Graduates
    {
        public Graduates()
        {
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
