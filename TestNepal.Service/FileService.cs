using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Collections;
using System.Configuration;
using TestNepal.Service.Infrastructure;

namespace TechNepal.Service
{
    /// <summary>
    /// It contains setting of Files
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// Saves base 64 Image Data
        /// </summary>
        /// <param name="FileData"></param>
        /// <returns></returns>
        public String SaveImage(String BasePath, String FileData)
        {
            if (!string.IsNullOrEmpty(FileData))
            {
                String PathStr = HttpContext.Current.Server.MapPath("~/" + BasePath);
                byte[] b = Convert.FromBase64String(FileData);
                MemoryStream ms = new System.IO.MemoryStream(b);
                Image img = Image.FromStream(ms);
                string fileExtention = ".jpg";
                fileExtention = GetFileExtension(img);
                String FileName = Guid.NewGuid().ToString() + fileExtention;
                img.Save(PathStr + FileName, img.RawFormat);
                
                img.Dispose();
                ms.Close();
                return FileName;
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BasePath"></param>
        /// <param name="httpPostedFile"></param>
        /// <returns></returns>
        public string SaveImage(string BasePath, HttpPostedFile httpPostedFile)
        {
            String PathStr = HttpContext.Current.Server.MapPath("~/" + BasePath);
            string fileExtention = ".jpg";
            String FileName = Guid.NewGuid().ToString() + fileExtention;
            httpPostedFile.SaveAs(PathStr + FileName);
            return FileName;
        }

        public String GetFileExtension(Image img)
        {
            if (img.RawFormat == ImageFormat.Jpeg)
            {
                return ".jpg";
            }
            return ".jpg";
        }
        

        /// <summary>
        /// Calculates resized dimensions for an image, preserving the aspect ratio.
        /// </summary>
        /// <param name="image">Image instance</param>
        /// <param name="desiredWidth">desired width</param>
        /// <param name="desiredHeight">desired height</param>
        /// <returns>Size instance with the resized dimensions</returns>
        private Size CalculateResizedDimensions(Image image, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / image.Width;
            var heightScale = (double)desiredHeight / image.Height;

            // scale to whichever ratio is smaller, this works for both scaling up and scaling down
            var scale = widthScale < heightScale ? widthScale : heightScale;

            return new Size
            {
                Width = (int)(scale * image.Width),
                Height = (int)(scale * image.Height)
            };
        }

        /// <summary>
        /// Delete image from web location
        /// </summary>
        /// <param name="ImageName">BasePath,PhotoPath</param>
        /// <returns></returns>
        public void DeleteImage(String BasePath, String PhotoPath)
        {
            if (PhotoPath != null)
            {
                if (PhotoPath != "noimage.jpeg")
                {
                    System.IO.FileInfo img = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(@"~\" + BasePath + PhotoPath));
                    if (img.Exists)
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(@"~\" + BasePath + PhotoPath));
                    }
                }
            }
        }

        /// <summary>
        /// check image exists or not on web location
        /// </summary>
        /// <param name="ImageName">BasePath,PhotoPath</param>
        /// <returns>ImageName</returns>
        public String CheckImage(String BasePath, String PhotoPath)
        {
            if (String.IsNullOrEmpty(PhotoPath))
                return "noimage.jpeg";
            else if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/" + BasePath + PhotoPath.Trim())))
                return PhotoPath.Trim();
            else
                return "noimage.jpeg";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public  bool IsBase64(string base64String)
        {
            if (base64String.Replace(" ", "").Length % 4 != 0)
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException ex)
            {
                // Handle the exception
            }
            return false;
        }

        /// <summary>
        /// Match the orientation code to the correct rotation
        /// </summary>
        /// <param name="orientationNum">Numeric value of orientation of image</param>
        /// <returns>Angle of rotation</returns>

        public RotateFlipType OrientationToFlipType(int orientationNum)
        {
            switch (orientationNum)
            {
                case 1:
                    return RotateFlipType.RotateNoneFlipNone;
                case 2:
                    return RotateFlipType.RotateNoneFlipX;
                case 3:
                    return RotateFlipType.Rotate180FlipNone;
                   
                case 4:
                    return RotateFlipType.Rotate180FlipX;
                   
                case 5:
                    return RotateFlipType.Rotate90FlipX;
                    
                case 6:
                    return RotateFlipType.Rotate90FlipNone;
                    
                case 7:
                    return RotateFlipType.Rotate270FlipX;
                    
                case 8:
                    return RotateFlipType.Rotate270FlipNone;
                   
                default:
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }

        public string ConvertToBase64FromUrl(string filePath)
        {
            String PathStr = HttpContext.Current.Server.MapPath("~/" + filePath);
            byte[] image = File.ReadAllBytes(PathStr);
            return Convert.ToBase64String(image);
        }

       
    }
}