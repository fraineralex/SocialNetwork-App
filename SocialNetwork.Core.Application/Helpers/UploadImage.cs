using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Core.Application.Helpers
{
    public class UploadImage
    {
        public static string UploadFile(IFormFile file, int id, string place = "Posts", bool isEditMode = false, string imageURL = null)
        {
            if (file != null)
            {

                if (isEditMode && file == null)
                {
                    return imageURL;
                }

                //get directory path
                string basePath = $"/Images/{id}/{place}";
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

                //create folder if not exist
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //get file path
                Guid guid = Guid.NewGuid();
                FileInfo fileInfo = new(file.FileName);
                string fileName = fileInfo.Name + fileInfo.Extension;
                string fileNameWhitPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWhitPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                if (isEditMode && imageURL != null)
                {
                    string[] oldImagePath = imageURL.Split("/");
                    string oldImageName = oldImagePath[^1];
                    string completeImageOldPath = Path.Combine(path, oldImageName);

                    if (System.IO.File.Exists(completeImageOldPath))
                    {
                        System.IO.File.Delete(completeImageOldPath);
                    }
                }

                return $"{basePath}/{fileName}";
            }
            else
            {
                if (isEditMode)
                {
                    return imageURL;
                }

                return null;
            }
        }
    }
}
