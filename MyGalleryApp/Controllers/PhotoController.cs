using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyGalleryApp.Data;
using MyGalleryApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyGalleryApp.Controllers

{
    [Authorize]
    public class PhotoController : Controller
    {
        ApplicationDbContext db;

        
        private IHostingEnvironment _oIHostingEvironment;
        
        public PhotoController(ApplicationDbContext db, IHostingEnvironment oIHostingEnvironment)
        {
            this.db = db;
            _oIHostingEvironment = oIHostingEnvironment;
            

        }
       
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PhotoViewModel viewModel = new PhotoViewModel();
            viewModel.photoList = db.Photo.ToList();
            viewModel.photoList = viewModel.photoList.Where(x => x.UserId == userId).ToList();
            viewModel.photo = new Photo();
            return View(viewModel);
        }

        public IActionResult SharedWithMe()
        {
            ShareViewModel model = new ShareViewModel();
            model.ShareList = db.Shared.ToList();


            return View(model);
        }
        public async Task<IActionResult>  Share(int id)
        {
            Share obj = new Share();

            var dbphoto = db.Photo.Find(id);
            obj.PhotoId = dbphoto.PhotoId;
            obj.Email = null;
            obj.Name = dbphoto.Name;
                
                    db.Shared.Add(obj);
                    await db.SaveChangesAsync();
                
           
           return RedirectToAction("SharedWithMe");
        }
        

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var photos = await db.Photo
                .FirstOrDefaultAsync(m => m.PhotoId == id);
            if (photos == null)
            {
                return NotFound();
            }

            return View(photos);
        }

        [HttpPost]
        public IActionResult AddPhotos(PhotoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var Files = model.photo.filePhoto;
            

            if (Files.Count > 0)
            {
                foreach (var item in Files)
                {
                    Photo photography = new Photo();
                    
                    var filePath = "wwwroot/photography/" + item.FileName;
                    var fileName = item.FileName;
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        item.CopyTo(stream);
                        photography.Name = fileName;
                        photography.Path = filePath;
                        photography.Title = item.FileName;
                        photography.CaptureBy = model.photo.CaptureBy;
                        photography.CaptureDate = model.photo.CaptureDate;
                        photography.Location = model.photo.Location;
                        photography.UserId = userId;
                        photography.Tag = model.photo.Tag;
                        db.Photo.Add(photography);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        public IActionResult DeletePhoto(int id)
        {
            var photo = db.Photo.Find(id);
            if (photo != null)
            {
                db.Photo.Remove(photo);
                db.SaveChanges();
            }
            return RedirectToAction("index");

        }
       
        // GET: Photos/Edit/5
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            Photo model = new Photo();
            var dbphoto = db.Photo.Find(id);
            model.Location = dbphoto.Location;
            model.Name = dbphoto.Name;
            model.Path = dbphoto.Path;
            model.PhotoId = dbphoto.PhotoId;
            model.Tag = dbphoto.Tag;
            model.Title = dbphoto.Title;
            model.UserId = dbphoto.UserId;
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        

    // POST: Photos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Photo photos)
        {
            if (photos == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(photos);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(photos);
        }

        public FileResult DownloadZip()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            PhotoViewModel viewModel = new PhotoViewModel();
            viewModel.photoList = db.Photo.ToList();
            viewModel.photoList = viewModel.photoList.Where(x => x.UserId == userId).ToList();
            var webRoot = _oIHostingEvironment.WebRootPath;
            var fileName = "MyPictures.zip";
            var temOutPut = webRoot + "/photography/" + fileName;

            using (ZipOutputStream oZipOutputStream = new ZipOutputStream(System.IO.File.Create(temOutPut)))
            {
                oZipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];
                var ImageList = new List<String>();
                foreach (var pname in viewModel.photoList)
                {
                    ImageList.Add(webRoot + "/photography/" + pname.Title);
                }

                for (int i = 0; i < ImageList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(ImageList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    oZipOutputStream.PutNextEntry(entry);

                    using (FileStream oFileStream = System.IO.File.OpenRead(ImageList[i]))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = oFileStream.Read(buffer, 0, buffer.Length);
                            oZipOutputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
                oZipOutputStream.Finish();
                oZipOutputStream.Flush();
                oZipOutputStream.Close();

            }
            byte[] finalResult = System.IO.File.ReadAllBytes(temOutPut);
            if (System.IO.File.Exists(temOutPut))
            {
                System.IO.File.Delete(temOutPut);
            }
            if (finalResult == null)
            {
                throw new Exception(String.Format("Nothing Found"));
            }
            return File(finalResult, "application/zip", fileName);
        }


    }
}
