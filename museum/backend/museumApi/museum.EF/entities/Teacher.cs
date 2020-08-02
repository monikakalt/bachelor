using System.Collections.Generic;

namespace museumApi.EF.entities
{
    public partial class Teacher
    {
        public Teacher()
        {
            Classes = new HashSet<ClassInfo>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }

        public ICollection<ClassInfo> Classes { get; set; }
    }
}
