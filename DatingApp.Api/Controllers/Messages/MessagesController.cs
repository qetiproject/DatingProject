using AutoMapper;
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
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IBaseRepository _repo;
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessagesController(
            IBaseRepository repo,
            IUserRepository userRepository,
            IMapper mapper, 
            IMessageRepository messageRepository
        )
        {
            _repo = repo;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageCreateDto messageCreateDto)
        {
            var sender = await _userRepository.GetUser(userId);
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageCreateDto.SenderId = userId;

            User recipient = await _userRepository.GetUser(messageCreateDto.RecipientId);

            if (recipient == null)
                return BadRequest("Could not find user");

            Message message = _mapper.Map<Message>(messageCreateDto);

            _repo.Add(message);

            if (await _repo.saveAll())
            {
                var messageToReturn = _mapper.Map<MessageDto>(message);
                return CreatedAtRoute("GetMessage", new { userId, id = message.Id }, messageToReturn);
            }
            throw new Exception("Creating the message failed on save");
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            Message message = await _messageRepository.GetMessage(id);

            if (message == null)
                return NotFound();

            return Ok(message);

        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            messageParams.UserId = userId;

            PagedList<Message> messagesFromRepo = await _messageRepository.GetMessagesForUser(messageParams);

            IEnumerable<MessageDto> messages = _mapper.Map<IEnumerable<MessageDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            Message message = await _messageRepository.GetMessage(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _repo.saveAll();

            return NoContent();
        }

        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            IEnumerable<Message> messageFromRepo = await _messageRepository.GetMessageThread(userId, recipientId);

            IEnumerable<MessageDto> messageThread = _mapper.Map<IEnumerable<MessageDto>>(messageFromRepo);

            return Ok(messageThread);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            Message message = await _messageRepository.GetMessage(id);

            if (message.SenderId == userId)
                message.SenderDeleted = true;

            if (message.RecipientId == userId)
                message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _repo.Delete(message);

            if (await _repo.saveAll())
                return NoContent();

            throw new Exception("Error deleting the message");
        }      
    }
}
