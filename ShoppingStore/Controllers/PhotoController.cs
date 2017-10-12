using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using ShoppingStore.Models.Dto;
using AutoMapper;
using ShoppingStore.Models;
using ShoppingStore.Data;

namespace ShoppingStore.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public PhotoController(IHostingEnvironment host, IMapper mapper)
        {
            this.host = host;
            this.mapper = mapper;
            dbContext = new ApplicationDbContext();
        }

        [HttpPost]
        [Route("api/upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var photos = dbContext.Photos.ToList();

            if (file == null)
            {
                return BadRequest("No upload image file");
            }

            if (file.Length == 0)
            {
                return BadRequest("Empty file");
            }

            if (file.Length > 10 * 1024 * 1024)
            {
                return BadRequest("Max file size exceeded");
            }

            // Check  repeated filename
            foreach (var p in photos)
            {
                if (p.FileName.Contains(file.FileName))
                {
                    return BadRequest("Your photo name has repeated");
                }
            }

            var uploadFolderPath = Path.Combine(host.WebRootPath, "photos");
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var fileName = file.FileName + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var photo = new Photo
            {
                FileName = fileName
            };

            dbContext.Photos.Add(photo);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        public async Task<IActionResult> UploadPhotos(IList<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size, filePath });
        }

        [HttpGet]
        [Route("api/photos")]
        public IActionResult GetPhotos()
        {

            var photos = dbContext.Photos.ToList();

            if (photos == null)
            {
                return NotFound();
            }

            List<string> photosPath = new List<string>();
            foreach (var p in photos)
            {
                photosPath.Add(host.WebRootFileProvider.GetFileInfo("photos/" + p.FileName)?.PhysicalPath);
            }

            return Ok(photosPath);
        }

        [HttpGet]
        [Route("api/photos/{photoid}")]
        public IActionResult GetPhoto(string photoId)
        {
            var photo = dbContext.Photos.FirstOrDefault(p => p.Id.ToString() == photoId);
            if (photo == null)
            {
                return NotFound();
            }

            var photoPath = host.WebRootFileProvider.GetFileInfo("photos/" + photo.FileName)?.PhysicalPath;

            return Ok(photoPath);

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}