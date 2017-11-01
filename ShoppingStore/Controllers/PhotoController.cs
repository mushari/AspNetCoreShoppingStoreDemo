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
using ShoppingStore.Models.ProductViewModels;
using ShoppingStore.Data.Repositories;
using ShoppingStore.Models.PhotoViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhotoController : Controller
    {
        private readonly IHostingEnvironment host;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPhotoRepository photoRepository;

        public PhotoController(
            IHostingEnvironment host,
            IMapper mapper,
            IPhotoRepository repository,
            IUnitOfWork unitOfWork)
        {
            this.host = host;
            this.mapper = mapper;
            this.photoRepository = repository;
            this.unitOfWork = unitOfWork;
        }


        public IActionResult Index(string returnUrl)
        {
            PhotoViewModel model = new PhotoViewModel
            {
                Photos = photoRepository.GetPhotos().ToList()
            };
            return View(model);
        }

        public IActionResult UploadView(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("api/uploadfile")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var photos = photoRepository.GetPhotos();

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

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            if (photos != null)
            {
                // Check  repeated filename
                foreach (var p in photos)
                {
                    if (p.FileName.Contains(fileName))
                    {
                        return BadRequest("Your photo name has repeated");
                    }
                }
            }

            await photoRepository.AddPhotoAsync(file, host);
            await unitOfWork.CompleteAsync();


            var photo = photoRepository.GetPhoto(fileName);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }


        [HttpGet]
        [Route("api/photos")]
        public IActionResult GetPhotos()
        {

            var photos = photoRepository.GetPhotos().ToList();

            if (photos == null)
            {
                return NotFound();
            }

            return Ok(photos);
        }

        [HttpDelete]
        [Route("api/deletePhoto")]
        public IActionResult DeletePhoto(string name)
        {
            var photo = photoRepository.GetPhoto(name);
            if (photo == null)
            {
                return NotFound();
            }

            photoRepository.DeletePhoto(photo, host);
            unitOfWork.Complete();
            return Ok(photo);
        }


    }
}