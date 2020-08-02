using AutoMapper;
using museum.Application.main.photos.dto;
using museum.EF.repositories;
using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.Application.main.photos
{
    public class PhotosService : IPhotosService
    {

        protected readonly IPhotosRepository _repository;
        protected readonly IMapper _mapper;
        public PhotosService(IPhotosRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<PhotoDTO> GetAllPhotos()
        {
            var list = _repository.GetAllPhotos();
            var listDto = _mapper.Map<List<PhotoDTO>>(list);
            return listDto;
        }

        public Photo PostPhoto(PhotoDTO item)
        {
            var photo = _mapper.Map<Photo>(item);
            _repository.PostPhoto(photo);
            return photo;
        }

        public void DeletePhoto(int id)
        {
            _repository.DeletePhoto(id);
        }

        public PhotoDTO GetPhotoById(int id)
        {
            var s = _repository.GetPhotoById(id);
            var dto = _mapper.Map<PhotoDTO>(s);
            return dto;
        }
    }
}
