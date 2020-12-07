using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Data.Base;
using DatingApp.Api.Data.Repositories;
using DatingApp.Api.Dtos;
using DatingApp.Api.Helpers;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBaseRepository _repo;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(
            IBaseRepository repo,
             IUserRepository userRepository,
            IMapper mapper
        )
        {
            _repo = repo;
            _userRepository = userRepository;
            _mapper = mapper;

        }


        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            PagedList<User> users = await _userRepository.GetUsers(userParams);

            IEnumerable<UsersDto> userToReturn = _mapper.Map<IEnumerable<UsersDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(userToReturn);
        }

        [HttpGet("filteredUsers")]
        public async Task<IActionResult> GetUsersFiltered([FromQuery] UserParams userParams)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            User user = await _userRepository.GetUser(userId);

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = user.Gender == "male" ? "female" : "male";
            }

            PagedList<User> users = await _userRepository.GetUsersFiltered(userParams);

            IEnumerable<UsersDto> userToReturn = _mapper.Map<IEnumerable<UsersDto>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(userToReturn);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            User user = await _userRepository.GetUser(id);
            UserDetailDto userToReturn = _mapper.Map<UserDetailDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            User user = await _userRepository.GetUser(id);

            _mapper.Map(userUpdate, user);

            if (await _repo.saveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);

            if (like != null)
                return BadRequest("You've already liked this user");

            if (await _userRepository.GetUser(recipientId) == null)
                return NotFound();

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (await _repo.saveAll())
                return Ok();

            return BadRequest("Failed to like user");
        }
    }
}
