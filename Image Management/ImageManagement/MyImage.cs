using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Policy;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static ImageManagement.Enums;

namespace ImageManagement
{
    public class MyImage
    {
        private FileInfo _sourceImageInfo;
        private Image _sourceImage;

        public int Width { get { return _sourceImage.Width; } }
        public int Height { get { return _sourceImage.Height; } }
        public String FileName { get { return _sourceImageInfo.Name; } }
        public String Extension { get { return System.IO.Path.GetExtension(_sourceImageInfo.FullName); } }
        public FileInfo File { get { return _sourceImageInfo; } }

        public MyImage(String imagePath)
        {
            _sourceImageInfo = new FileInfo(imagePath);
            _sourceImage = Image.FromFile(_sourceImageInfo.FullName);
        }

        public void ResizeImage(int width, int height)
        {
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)_sourceImage.Width);
            nPercentH = ((float)height / (float)_sourceImage.Height);

            nPercent = Math.Min(nPercentW, nPercentH);

            // New Width and Height
            int destWidth = (int)(_sourceImage.Width * nPercent);
            int destHeight = (int)(_sourceImage.Height * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.High;

            // Draw image with new width and height
            g.DrawImage(_sourceImage, 0, 0, destWidth, destHeight);
            //g.Save();
            g.Dispose();
        }

        public void GrayedImage()
        {
            var grayscale = new System.Drawing.Imaging.ColorMatrix(new float[][] { new float[] { 0.3f, 0.3f, 0.3f, 0f, 0f }, new float[] { 0.59f, 0.59f, 0.59f, 0f, 0f }, new float[] { 0.11f, 0.11f, 0.11f, 0f, 0f }, new float[] { 0f, 0f, 0f, 1f, 0f }, new float[] { 0f, 0f, 0f, 0f, 1f } });

            var bitmapObj = new Bitmap(_sourceImage);
            var grayImageAttr = new System.Drawing.Imaging.ImageAttributes();
            grayImageAttr.SetColorMatrix(grayscale);

            using (Graphics g = Graphics.FromImage(bitmapObj))
            {
                g.DrawImage(bitmapObj, new Rectangle(0, 0, bitmapObj.Width, bitmapObj.Height), 0, 0, bitmapObj.Width, bitmapObj.Height, GraphicsUnit.Pixel, grayImageAttr);
            }
        }

        public void CompressImage(SnapshotQuality snapshotQuality)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            var imageEncoder = (from itm in codecs
                             where itm.FormatID == ImageFormat.Jpeg.Guid
                             select itm).First();

            using (var fs = new FileStream(_sourceImageInfo.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var srcImage = Image.FromStream(fs))
                {
                    using (var qualityEncoder = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, getQualityValue(snapshotQuality)))
                    {
                        using (var encoderParameters = new EncoderParameters() { Param = new[] { qualityEncoder } })
                        {
                            srcImage.Save(fs, imageEncoder, encoderParameters);
                        }
                    }
                }
            }
        }

        private int getQualityValue(SnapshotQuality snapshotQuality)
        {
            switch (snapshotQuality)
            {
                case SnapshotQuality.Low:
                    {
                        return 10;
                    }

                case SnapshotQuality.Medium:
                    {
                        return 35;
                    }

                case SnapshotQuality.High:
                    {
                        // most JPG source files are gnerated at 70% quality. 
                        // hence regenerating the file at a higher quality 
                        return 70;
                    }

                default:
                    {
                        throw new NotImplementedException(snapshotQuality.ToString());
                    }
            }
        }

        public void DrawWatermark(DateTime watermark)
        {
            string text = watermark.ToString("dd MMM yyyy, HH:mm:ss zzz");

            using (var g2d = Graphics.FromImage(_sourceImage))
            {
                var point = new Point(_sourceImage.Width - 70, _sourceImage.Height - 70);
                var font = new Font("arial", (float)14.0d, FontStyle.Bold, GraphicsUnit.Point);
                var StrFormat = new StringFormat();
                StrFormat.Alignment = StringAlignment.Far;

                var semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
                g2d.DrawString(text, font, semiTransBrush2, new PointF(point.X + 1, point.Y + 1), StrFormat);

                var semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
                g2d.DrawString(text, font, semiTransBrush, point, StrFormat);
            }
        }


    }
}
