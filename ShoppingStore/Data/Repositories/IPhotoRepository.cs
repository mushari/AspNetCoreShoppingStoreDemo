using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Data.Repositories
{
    public interface IPhotoRepository
    {
        IEnumerable<Photo> GetPhotos();
        Photo GetPhoto(string name);
       
        Task AddPhotoAsync(IFormFile file,IHostingEnvironment host);
        
        void DeletePhoto(Photo photo, IHostingEnvironment host);
    }
}
