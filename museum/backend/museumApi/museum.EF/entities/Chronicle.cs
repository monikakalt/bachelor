using System;
using System.Collections.Generic;

namespace museumApi.EF.entities
{
    public partial class Chronicle
    {
        public Chronicle()
        {
            Photos = new HashSet<Photo>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public string FolderUrl { get; set; }
        public int? FkUser { get; set; }

        public User FkUserNavigation { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
