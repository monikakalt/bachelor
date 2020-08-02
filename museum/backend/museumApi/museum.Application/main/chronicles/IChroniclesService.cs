using Microsoft.AspNetCore.Http;
using museum.Application.main.chronicles.dto;
using museum.Application.main.photos.dto;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main.chronicles
{
    public interface IChroniclesService
    {
        IEnumerable<ChronicleDTO> GetAllChronicles();
        ChronicleDTO GetRecentChronicle();
        ChronicleDTO GetChronicleById(int id);
        void DeleteChronicle(int id);
        Chronicle PostChronicle(ChronicleDTO chronicle);
        void UpdateChronicle(int id, ChronicleDTO c);
        void ProcessFiles(IFormFileCollection files, string folderName, int chronicleId);
        void DeletePhotos(int id);
    }
}
