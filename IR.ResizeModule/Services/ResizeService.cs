using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.IO.Compression;
using IR.Infrastructure.Interfaces;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;

namespace IR.ResizeModule.Services
{
    public class ResizeService: IResizeService
    {
        private readonly ObservableCollection<string> resizedImages;
        private readonly string[] imageExtensions = {".jpg", ".jpeg", ".png", ".gif", ".tiff", ".bmp"};
        private Bitmap image;

        public ResizeService()
        {
            resizedImages = new ObservableCollection<string>();
        }

        public ObservableCollection<string> RetrieveResizedImages()
        {
            return resizedImages;
        }

        public void Resize(string path, int maxWidth, int maxHeight, int quality, bool saveSourceImage = true)
        {
            resizedImages.Clear();

            var files = new List<string>();
            if (Directory.Exists(path))
            {
                foreach (var imageExtension in imageExtensions)
                {
                    files.AddRange(Directory.GetFiles(path, string.Format("*{0}", imageExtension), SearchOption.AllDirectories));
                }
            }
            else if (File.Exists(path) && imageExtensions.Contains(Path.GetExtension(path) ?? string.Empty, StringComparer.InvariantCultureIgnoreCase))
                files.Add(path);

            if (saveSourceImage && files.Any())
            {
                try
                {
                    var tempFolder = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));
                    Directory.CreateDirectory(tempFolder);
                    foreach (var file in files)
                    {
                        var destFileName = string.Format("{0}{1}", 
                            tempFolder,
                            file.Replace(Path.GetDirectoryName(path) ?? string.Empty, string.Empty));

                        var destFolder = Path.GetDirectoryName(destFileName);
                        if (!string.IsNullOrEmpty(destFolder) && !Directory.Exists(destFolder))
                            Directory.CreateDirectory(destFolder);

                        File.Copy(file, destFileName);
                    }
                    var destinationArchiveFileName = Path.Combine(Path.GetDirectoryName(path) ?? string.Empty,
                        string.Format("{0}.{1:yyyy.MM.dd hhmmss}.zip", Path.GetFileNameWithoutExtension(path), DateTime.Now));

                    if (File.Exists(destinationArchiveFileName))
                        File.Delete(destinationArchiveFileName);

                    ZipFile.CreateFromDirectory(tempFolder, destinationArchiveFileName);
                    Directory.Delete(tempFolder, true);
                }
                catch (Exception exception)
                {
                    Logger.Log(exception.GetRootException().Message, Category.Exception, Priority.High);
                    return;
                }
            }

            foreach (var file in files)
            {
                try
                {
                    Save(file, maxWidth, maxHeight, quality);
                    resizedImages.Add(file);
                }
                catch (Exception exception)
                {
                    Logger.Log(exception.GetRootException().Message, Category.Exception, Priority.High);
                }
            }
        }

        private void Save(string imageFile, int maxWidth, int maxHeight, int quality)
        {
            using (var memoryStream = new MemoryStream(File.ReadAllBytes(imageFile)))
            {
                image = new Bitmap(memoryStream);
            }
            var originalWidth = image.Width;
            var originalHeight = image.Height;
            
            // To preserve the aspect ratio
            var ratioX = (double)maxWidth / originalWidth;
            var ratioY = (double)maxHeight / originalHeight;
            var ratio = Math.Min(ratioX, ratioY);
            if (ratio >= 1.0)
                return;

            // New width and height based on aspect ratio
            var newWidth = (int)(originalWidth * ratio);
            var newHeight = (int)(originalHeight * ratio);
 
            // Convert other formats (including CMYK) to RGB.
            using (var newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb))
            {
                newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                    image.Dispose();
                }

                // Get an ImageCodecInfo object that represents the JPEG codec.
                var imageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg);

                // Create an Encoder object for the Quality parameter.
                var encoder = Encoder.Quality;

                // Create an EncoderParameters object. 
                var encoderParameters = new EncoderParameters(1);

                // Save the image as a JPEG file with quality level.
                var encoderParameter = new EncoderParameter(encoder, quality);
                encoderParameters.Param[0] = encoderParameter;
                newImage.Save(imageFile, imageCodecInfo, encoderParameters);
            }
        }

        private static ImageCodecInfo GetEncoderInfo(ImageFormat imageFormat)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == imageFormat.Guid);
        }

        [Dependency]
        public ILoggerFacade Logger { get; set; }
    }
}
