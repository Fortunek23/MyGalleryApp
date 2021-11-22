using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyGalleryApp.Data
{
    public class Photo
    {
        [Key]
        public int PhotoId { get; set; }
        public string UserId { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public string Tag  { get; set; }
        public string  CaptureBy { get; set; }
        public DateTime CaptureDate { get; set; }
        public string Path { get; set; }

        [NotMappedAttribute]
        [Required(ErrorMessage = "Photo is required.")]
        public List<IFormFile> filePhoto { get; set; }

    }
}
