using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGalleryApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<SharedWithMe> sharedWithMe { get; set; }

        public virtual DbSet<Share> Shared { get; set; }
    }
}
