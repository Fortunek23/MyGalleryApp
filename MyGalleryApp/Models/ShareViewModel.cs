using MyGalleryApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGalleryApp.Models
{
    public class ShareViewModel
    {
        public Share Share { get; set; }
        public List<Share> ShareList { get; set; }
    }
}
