using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGalleryApp.Data
{
    public class Share
    {
        public int ShareId { get; set; }

        public int PhotoId { get; set; }

        public Photo Photo { get; set; }
        public string Email { get; set; }

        public ICollection<Photo> Photos { get; set; }

    }
}
