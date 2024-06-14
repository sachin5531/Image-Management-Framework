using ImageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebGrease.Activities;
using static ImageManagement.Enums;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult ResizeImage([FromBody]int width, [FromBody]int heigth)
        {
            //Guid sessionId = Guid.NewGuid();

            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No File provided." });
            }
            else
            {
                HttpPostedFileBase selectedFile = Request.Files[0];
                var sourceFile = System.IO.Path.GetTempFileName();
                selectedFile.SaveAs(sourceFile);

                MyImage myImageObj = new MyImage(sourceFile);
                myImageObj.ResizeImage(width, heigth);

                byte[] fileContents = System.IO.File.ReadAllBytes(myImageObj.File.FullName);

                return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, myImageObj.FileName);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult GrayedImage()
        {
            //Guid sessionId = Guid.NewGuid();

            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No File provided." });
            }
            else
            {
                HttpPostedFileBase selectedFile = Request.Files[0];
                var sourceFile = System.IO.Path.GetTempFileName();
                selectedFile.SaveAs(sourceFile);

                MyImage myImageObj = new MyImage(sourceFile);
                myImageObj.GrayedImage();

                byte[] fileContents = System.IO.File.ReadAllBytes(myImageObj.File.FullName);

                return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, myImageObj.FileName);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult CompressImage([FromBody] String imageQuality)
        {
            SnapshotQuality imgQuality = (SnapshotQuality)Enum.Parse(typeof (SnapshotQuality), imageQuality);

            //Guid sessionId = Guid.NewGuid();

            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No File provided." });
            }
            else
            {
                HttpPostedFileBase selectedFile = Request.Files[0];
                var sourceFile = System.IO.Path.GetTempFileName();
                selectedFile.SaveAs(sourceFile);

                MyImage myImageObj = new MyImage(sourceFile);
                myImageObj.CompressImage(imgQuality);

                byte[] fileContents = System.IO.File.ReadAllBytes(myImageObj.File.FullName);

                return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, myImageObj.FileName);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult DrawWatermark([FromBody] DateTime watermarkTimeStamp)
        {
             //Guid sessionId = Guid.NewGuid();

            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No File provided." });
            }
            else
            {
                HttpPostedFileBase selectedFile = Request.Files[0];
                var sourceFile = System.IO.Path.GetTempFileName();
                selectedFile.SaveAs(sourceFile);

                MyImage myImageObj = new MyImage(sourceFile);
                myImageObj.DrawWatermark(watermarkTimeStamp);

                byte[] fileContents = System.IO.File.ReadAllBytes(myImageObj.File.FullName);

                return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, myImageObj.FileName);
            }
        }

        [System.Web.Http.HttpPost]
        public ActionResult ProcessMultipleImages([FromBody] DateTime watermarkTimeStamp)
        {
            //Guid sessionId = Guid.NewGuid();

            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No File provided." });
            }
            else
            {
                HttpPostedFileBase selectedFile = Request.Files[0];
                var sourceFile = System.IO.Path.GetTempFileName();
                selectedFile.SaveAs(sourceFile);

                MyImage myImageObj = new MyImage(sourceFile);
                myImageObj.DrawWatermark(watermarkTimeStamp);

                byte[] fileContents = System.IO.File.ReadAllBytes(myImageObj.File.FullName);

                return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, myImageObj.FileName);
            }
        }



    }


}
