using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingStore.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ShoppingStore.Data.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {

        private ApplicationDbContext context;
        public PhotoRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void DeletePhoto(Photo photo, IHostingEnvironment host)
        {
            context.Photos.Remove(photo);
            var uploadFolderPath = host.WebRootPath + photo.FileAddress;
            System.IO.File.Delete(uploadFolderPath);
        }

        public IEnumerable<Photo> GetPhotos()
        {
            return context.Photos;
        }

        public Photo GetPhoto(string name)
        {
            return context.Photos.SingleOrDefault(p => p.FileName == name);
        }

        public async Task AddPhotoAsync(IFormFile file, IHostingEnvironment host)
        {
            var uploadFolderPath = Path.Combine(host.WebRootPath, "photos");
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var filePath = Path.Combine(uploadFolderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo
            {
                FileName = fileName,
                FileAddress = "/photos/" + file.FileName
            };
            context.Photos.Add(photo);
        }


    }
}
