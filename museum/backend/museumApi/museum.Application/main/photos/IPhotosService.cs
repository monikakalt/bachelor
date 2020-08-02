using museum.Application.main.photos.dto;
using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace museum.Application.main.photos
{
    public interface IPhotosService
    {
        IEnumerable<PhotoDTO> GetAllPhotos();
        PhotoDTO GetPhotoById(int id);
        void DeletePhoto(int id);
        Photo PostPhoto(PhotoDTO item);
    }
}
