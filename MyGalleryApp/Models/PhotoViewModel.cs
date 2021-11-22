using MyGalleryApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGalleryApp.Models
{
    public class PhotoViewModel
    {
        public Photo photo { get; set; }
        public List<Photo> photoList { get; set; }
    }
}
