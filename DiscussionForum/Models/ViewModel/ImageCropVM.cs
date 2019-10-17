using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.IO;

namespace DiscussionForum.Models.ViewModel
{
    public class ImageCropVM
    {
        public HttpPostedFileBase ImageFile { get; set; }
        public int XCoor { get; set; }
        public int YCoor { get; set; }
        public int CropWidth { get; set; }
        public int CropHeight { get; set; }

        public ImageCropVM()
        {

        }

        public Bitmap CropAndSaveImage()
        {
            Bitmap croppedImage = new Bitmap(1, 1);
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(ImageFile.FileName);
                Stream stream = ImageFile.InputStream;
                BinaryReader streamReader = new BinaryReader(stream);
                byte[] imageBytes = streamReader.ReadBytes((int)stream.Length);
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                {
                    croppedImage = new Bitmap(stream);
                }
            }


            return croppedImage;
        }
    }
}