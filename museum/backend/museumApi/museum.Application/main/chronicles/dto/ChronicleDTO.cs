using museum.Application.main.photos.dto;
using System;
using System.Collections.Generic;

namespace museum.Application.main.chronicles.dto
{
    public class ChronicleDTO
    { 
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public string FolderUrl { get; set; }
        public int? FkUser { get; set; }
        public IEnumerable<PhotoDTO> Photos { get; set; }
    }
}
