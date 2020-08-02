using AutoMapper;
using Microsoft.AspNetCore.Http;
using museum.Application.main.chronicles.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace museum.Application.main.chronicles
{
    public class ChroniclesService : IChroniclesService
    {
        protected readonly IChroniclesRepository _repository;
        protected readonly IPhotosRepository _photosRepository;
        protected readonly IMapper _mapper;
        public ChroniclesService(IChroniclesRepository repository, IMapper mapper, IPhotosRepository photosRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _photosRepository = photosRepository;
        }

        public IEnumerable<ChronicleDTO> GetAllChronicles()
        {
            var list = _repository.GetAll();
            var listDto = _mapper.Map<List<ChronicleDTO>>(list);
            return listDto;
        }

        public ChronicleDTO GetRecentChronicle()
        {
            var s = _repository.GetRecent();
            var dto = _mapper.Map<ChronicleDTO>(s);
            return dto;
        }

        public ChronicleDTO GetChronicleById(int id)
        {
            var s = _repository.GetById(id);
            var dto = _mapper.Map<ChronicleDTO>(s);
            return dto;
        }

        public Chronicle PostChronicle(ChronicleDTO item)
        {
            var sub = _mapper.Map<Chronicle>(item);
            _repository.Post(sub);
            return sub;
        }

        public void DeleteChronicle(int id)
        {
            var photos = _photosRepository.GetAllPhotos().FindAll(x => x.FkChronicle == id);
            if (photos.Count > 0)
            {
                foreach (var p in photos)
                {
                    _photosRepository.DeletePhoto(p.Id);
                }
            }
            _repository.Delete(id);
        }

        public void DeletePhotos(int id)
        {
            var photos = _photosRepository.GetAllPhotos().FindAll(x => x.FkChronicle == id);
            if (photos.Count > 0)
            {
                foreach (var p in photos)
                {
                    _photosRepository.DeletePhoto(p.Id);
                }
            }
        }

        public void UpdateChronicle(int id, ChronicleDTO c)
        {
            var sub = _mapper.Map<Chronicle>(c);
            _repository.Update(id, sub);
        }

        public void ProcessFiles(IFormFileCollection files, string folderName, int chronicleId)
        {
            if (!Directory.Exists(folderName))
            {
                //Directory.CreateDirectory(folderName);
            }
            var pathToSave = "D:\\KTU\\4 KURSAS 2 SEMESTRAS\\bakalauras\\jjgmuziejus\\museum\\museum-frontend\\src\\chronicles";

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(files[i].ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);

                        var photo = new Photo
                        {
                            Title = fileName,
                            Url = fullPath,
                            FkChronicle = chronicleId
                        };

                        _photosRepository.PostPhoto(photo);
                        if (!File.Exists(fullPath))
                        {
                            using (var stream = new FileStream(fullPath, FileMode.Create))
                            {

                                files[i].CopyTo(stream);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
