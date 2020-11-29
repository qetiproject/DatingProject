﻿using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IdatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IdatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var userToReturn = _mapper.Map<IEnumerable<UsersDto>>(users);
            return Ok(userToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserDetailDto>(user);
            return Ok(userToReturn);
        }
    }
}
