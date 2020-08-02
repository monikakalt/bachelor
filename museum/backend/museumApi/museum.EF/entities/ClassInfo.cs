namespace museumApi.EF.entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public partial class ClassInfo
    {
            public ClassInfo()
            {
                Students = new HashSet<Student>();
            }

            public int Id { get; set; }
            public string Title { get; set; }
            public int? FkTeacher { get; set; }

            public Teacher FkTeacherNavigation { get; set; }
            public ICollection<Student> Students { get; set; }
    }
}