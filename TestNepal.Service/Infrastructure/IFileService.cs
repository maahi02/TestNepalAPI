using TestNepal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Service.Infrastructure
{
    public interface IFileService
    {
        String SaveImage(String BasePath, String FileData);
        String SaveImage(String BasePath, System.Web.HttpPostedFile httpPostedFile);
        String CheckImage(String BasePath, String PhotoPath);
        void DeleteImage(String BasePath, String PhotoPath);
        string ConvertToBase64FromUrl(string filePath);
    }
}
