using MyGalleryApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGalleryApp.Models
{
    public class SharedViewModel
    {
        public SharedWithMe sharedWithMe { get; set; }
        public List<SharedWithMe> sharedWithMeList { get; set; }
    }
}
