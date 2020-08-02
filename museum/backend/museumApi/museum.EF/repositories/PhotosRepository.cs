using museumApi.EF.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace museum.EF.repositories
{
    public class PhotosRepository: Repository<Photo>, IPhotosRepository
    {
        public PhotosRepository(MuseumContext context) : base(context)
        {
        }

        public List<Photo> GetAllPhotos()
        {
            return museumContext.Photos
              .ToList();
        }

        public Photo GetPhotoById(int id)
        {
            return museumContext.Photos.Where(p => id == p.Id).FirstOrDefault();
        }

        public void PostPhoto(Photo c)
        {
            museumContext.Add(c);
            museumContext.SaveChanges();
        }

        public void DeletePhoto(int id)
        {
            var c = museumContext.Photos.SingleOrDefault(p => p.Id == id);
            museumContext.Remove(c);
            museumContext.SaveChanges();
        }

        public void UpdatePhoto(int id, Photo c)
        {
            var photo = museumContext.Photos.Where(p => id == p.Id).Single();
            photo.Title = c.Title;
            photo.Url = c.Url;
            museumContext.SaveChanges();
        }

        public MuseumContext museumContext
        {
            get { return Context as MuseumContext; }
        }
    }
}
