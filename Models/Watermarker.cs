using System.Drawing;
using Watermark.Enums;
using System.ComponentModel;
using System.Drawing.Imaging;
using System;

namespace Watermark.Models
{
    public class Watermarker
    {
        public Image Image { get; set; }

        public Image OriginalImage { get; set; }

        public Image Watermark { get; set; }

        public float Opacity { get; set; } = 1.0f;

        public WatermarkPosition Position { get; set; } = WatermarkPosition.Absolute;

        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public Color TransparentColor { get; set; } = Color.Empty;

        public RotateFlipType RotateFlip { get; set; } = RotateFlipType.RotateNoneFlipNone;

        //private Padding m_margin = new Padding(0);

        public float ScaleRatio { get; set; } = 1.0f;

        public Font Font { get; set; } = new Font(FontFamily.GenericSansSerif, 10);

        public Color FontColor { get; set; } = Color.Black;

        public Watermarker(Image image)
        {
            LoadImage(image);
        }

        public Watermarker(string fileNmae)
        {
            LoadImage(Image.FromFile(fileNmae));
        }

        public void ResetImage()
        {
            Image = new Bitmap(OriginalImage);
        }

        public void DrawImage(string fileName)
        {
            DrawImage(Image.FromFile(fileName));
        }

        public void DrawImage(Image watermark)
        {
            if (watermark == null)
            {
                throw new ArgumentOutOfRangeException("Watermark");
            }

            if (Opacity < 0 || Opacity > 1)
            {
                throw new ArgumentOutOfRangeException("Opacity");
            }

            if (ScaleRatio <= 0)
            {
                throw new ArgumentOutOfRangeException("ScaleRatio");
            }

            //Watermark = GetWatermarkImage(watermark);
            Watermark.RotateFlip(RotateFlip);

            Point watermarkPosition = GetWatermarkPosition();
            var destinationRectangle = new Rectangle(watermarkPosition.X, watermarkPosition.Y, Watermark.Width, Watermark.Height);
            var colorMatrix = new ColorMatrix(
                new float[][] {
                    new float[] { 1, 0f, 0f, 0f, 0f},
                    new float[] { 0f, 1, 0f, 0f, 0f},
                    new float[] { 0f, 0f, 1, 0f, 0f},
                    new float[] { 0f, 0f, 0f, Opacity, 0f},
                    new float[] { 0f, 0f, 0f, 0f, 1}
                });

            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            if (TransparentColor != Color.Empty)
            {
                attributes.SetColorKey(TransparentColor, TransparentColor);
            }

            using var graphics = Graphics.FromImage(Image);
            graphics.DrawImage(Watermark, destinationRectangle, 0, 0, Watermark.Width, Watermark.Height, GraphicsUnit.Pixel, attributes);
        }

        public void DrawText(string text)
        {
            Image watermarkText = GetWatermarkText(text);
            DrawImage(watermarkText);
        }

        private void LoadImage(Image image)
        {
            OriginalImage = image;
            ResetImage();
        }

        private Image GetWatermarkText(string text)
        {
            var brush = new SolidBrush(FontColor);
            SizeF size;

            using var gr1 = Graphics.FromImage(Image);
            size = gr1.MeasureString(text, Font);

            var bitmap = new Bitmap((int)size.Width, (int)size.Height);
            bitmap.SetResolution(Image.HorizontalResolution, Image.VerticalResolution);

            using var gr2 = Graphics.FromImage(bitmap);
            gr2.DrawString(text, Font, brush, 0, 0);

            return bitmap;
        }

        //private Image GetWatermarkImage(Image watermark)
        //{
        //    // If there are no margins specified and scale ration is 1, no need to create a new bitmap
        //    //if (m_margin.All == 0 && m_scaleRatio == 1.0f)
        //    //    return watermark;

        //    //// Create a new bitmap with new sizes (size + margins) and draw the watermark
        //    //int newWidth = Convert.ToInt32(watermark.Width * m_scaleRatio);
        //    //int newHeight = Convert.ToInt32(watermark.Height * m_scaleRatio);

        //    //Rectangle sourceRect = new Rectangle(m_margin.Left, m_margin.Top, newWidth, newHeight);
        //    //Rectangle destRect = new Rectangle(0, 0, watermark.Width, watermark.Height);

        //    //Bitmap bitmap = new Bitmap(newWidth + m_margin.Left + m_margin.Right, newHeight + m_margin.Top + m_margin.Bottom);
        //    //bitmap.SetResolution(watermark.HorizontalResolution, watermark.VerticalResolution);

        //    //using (Graphics g = Graphics.FromImage(bitmap))
        //    //{
        //    //    g.DrawImage(watermark, sourceRect, destRect, GraphicsUnit.Pixel);
        //    //}

        //    //return bitmap;
        //}

        private Point GetWatermarkPosition()
        {
            int x = 0;
            int y = 0;

            switch (Position)
            {
                case WatermarkPosition.Absolute:
                    x = X; y = Y;
                    break;
                case WatermarkPosition.TopLeft:
                    x = 0; y = 0;
                    break;
                case WatermarkPosition.TopRight:
                    x = Image.Width - Watermark.Width; y = 0;
                    break;
                case WatermarkPosition.TopMiddle:
                    x = (Image.Width - Watermark.Width) / 2; y = 0;
                    break;
                case WatermarkPosition.BottomLeft:
                    x = 0; y = Image.Height - Watermark.Height;
                    break;
                case WatermarkPosition.BottomRight:
                    x = Image.Width - Watermark.Width; y = Image.Height - Watermark.Height;
                    break;
                case WatermarkPosition.BottomMiddle:
                    x = (Image.Width - Watermark.Width) / 2; y = Image.Height - Watermark.Height;
                    break;
                case WatermarkPosition.MiddleLeft:
                    x = 0; y = (Image.Height - Watermark.Height) / 2;
                    break;
                case WatermarkPosition.MiddleRight:
                    x = Image.Width - Watermark.Width; y = (Image.Height - Watermark.Height) / 2;
                    break;
                case WatermarkPosition.Center:
                    x = (Image.Width - Watermark.Width) / 2; y = (Image.Height - Watermark.Height) / 2;
                    break;
                default:
                    break;
            }

            return new Point(x, y);
        }

    }
}
