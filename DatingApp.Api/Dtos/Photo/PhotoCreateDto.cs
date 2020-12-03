using Microsoft.AspNetCore.Http;
using System;

namespace DatingApp.Api.Dtos
{
    public class PhotoCreateDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }

        public PhotoCreateDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}
