using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Api.Data;
using DatingApp.Api.Data.Base;
using DatingApp.Api.Data.Repositories;
using DatingApp.Api.Dtos;
using DatingApp.Api.Helpers;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IBaseRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(
            IBaseRepository repo,
            IUserRepository userRepository,
            IPhotoRepository photoRepository,
            IMapper mapper, 
            IOptions<CloudinarySettings> cloudinaryConfig
        )
        {
            _repo = repo;
            _userRepository = userRepository;
            _photoRepository = photoRepository;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            Photo photoFromRepo = await _photoRepository.GetPhoto(id);

            PhotoDto photo = _mapper.Map<PhotoDto>(photoFromRepo);

            if (photo == null)
                return BadRequest("This photo doesn't exist");

            return Ok(photo);
        }


        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoCreateDto photoCreateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            User user = await _userRepository.GetUser(userId);

            var file = photoCreateDto.File;

            ImageUploadResult uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    ImageUploadParams uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoCreateDto.Url = uploadResult.Url.ToString();
            photoCreateDto.PublicId = uploadResult.PublicId;

            Photo photo = _mapper.Map<Photo>(photoCreateDto);

            if (!user.Photos.Any(user => user.IsMain))
                photo.IsMain = true;

            user.Photos.Add(photo);


            if (await _repo.saveAll())
            {
                var photoForReturn = _mapper.Map<PhotoDto>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id}, photoForReturn);
            }

            return BadRequest("Could not add the photo");
        }

        
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            User user = await _userRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized();

            Photo photo = await _photoRepository.GetPhoto(id);

            if (photo.IsMain)
                return BadRequest("This is already the main photo");

            Photo cuurentMainPhoto = await _photoRepository.GetmainPhotoForUser(userId);

            cuurentMainPhoto.IsMain = false;

            photo.IsMain = true;

            if (await _repo.saveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            User user = await _userRepository.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return BadRequest($"This photo is not for this user - {user.UserName}");

            Photo photo = await _photoRepository.GetPhoto(id);

            if (photo.IsMain)
                return BadRequest("You can not delete your main photo");

            if(photo.PublicId != null)
            {
                DeletionParams deleteParams = new DeletionParams(photo.PublicId);

                DeletionResult result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                    _repo.Delete(photo);
            }

            if(photo.PublicId == null)
            {
                _repo.Delete(photo);
            }

            if (await _repo.saveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}
