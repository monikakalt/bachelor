using museumApi.EF.entities;
using System.Collections.Generic;

namespace museum.EF.repositories
{
    public interface IPhotosRepository
    {
        List<Photo> GetAllPhotos();
        void PostPhoto(Photo c);
        void DeletePhoto(int id);
        void UpdatePhoto(int id, Photo c);
        Photo GetPhotoById(int id);
    }
}