using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = iTextSharp.text.Image;
using Rectangle = System.Drawing.Rectangle;

namespace PlantsWpf.PdfDocuments
{
    public class PdfDocumentCreator
    {
        public Document CreatePdfDocument(Chart chart, string path)
        {
            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                (int) chart.ActualWidth,
                (int) chart.ActualHeight,
                96d,
                96d,
                PixelFormats.Pbgra32);

            renderBitmap.Render(chart);

            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            encoder.Save(stream);

            Bitmap bitmap = new Bitmap(stream);
            System.Drawing.Image image = bitmap;

            System.Drawing.Image resizedImage = ResizeImage(image, image.Width*2, image.Height);

            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.OpenOrCreate));
            doc.Open();
            Image pdfImage = Image.GetInstance(resizedImage, ImageFormat.Jpeg);
            doc.Add(pdfImage);
            doc.Close();
            
            return doc;
        }
        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}