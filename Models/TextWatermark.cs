using System.Drawing;
using System.Drawing.Drawing2D;
using Watermark.Enums;

namespace Watermark.Models
{
    public static class TextWatermark
    {
        public static void AddTextWatermark(this Image image, string text, TextWatermarkOptions options)
        {
            using var graphics = Graphics.FromImage(image);
            var backgroundPosition = GetBackgrounPosition(image.Width, image.Height, options.FontSize, options.Margin, options.WatermarkPosition);
            var stringFormat = new StringFormat
            {
                FormatFlags = StringFormatFlags.NoWrap
            };

            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            if (options.BackgrounColor.A > 0)
            {
                var backgroundBrush = new SolidBrush(options.BackgrounColor);
                graphics.FillRectangle(backgroundBrush, backgroundPosition);
            }

            var fontFamily = new FontFamily(options.FontName);
            var font = new Font(fontFamily, options.FontSize, GraphicsUnit.Pixel);

            var textMetrics = graphics.MeasureString(text, font, image.Width, stringFormat);
            var beforeText = GetTextAlign(textMetrics, image.Width, options.WatermarkPosition);
            var drawPoint = new PointF(beforeText, backgroundPosition.Y + (backgroundPosition.Height / 4));

            var outlineBrush = new SolidBrush(options.OutlineColor);

            using var pen = new Pen(outlineBrush, options.OutlineWidth);
            using var graphicsPath = new GraphicsPath();

            graphicsPath.AddString(text, fontFamily, (int)options.FontStyle, options.FontSize, drawPoint, stringFormat);

            if (options.OutlineColor.A > 0)
            {
                graphics.DrawPath(pen, graphicsPath);
            }

            if (options.TextColor.A > 0)
            {
                var textBrush = new SolidBrush(options.TextColor);
                graphics.FillPath(textBrush, graphicsPath);
            }
        }

        private static Rectangle GetBackgrounPosition(int imageWidth, int imageHeight, int fontSize, int margin, WatermarkPosition watermarkPosition)
        {
            Rectangle rectangle;
            var backgroundHeight = fontSize * 2;

            switch (watermarkPosition)
            {
                case WatermarkPosition.TopLeft:
                case WatermarkPosition.TopMiddle:
                case WatermarkPosition.TopRight:
                    rectangle = new Rectangle(0, margin, imageWidth, backgroundHeight);
                    break;
                case WatermarkPosition.MiddleLeft:
                case WatermarkPosition.Center:
                case WatermarkPosition.MiddleRight:
                    rectangle = new Rectangle(0, imageHeight / 2 - backgroundHeight / 2, imageWidth, backgroundHeight);
                    break;
                case WatermarkPosition.BottomLeft:
                case WatermarkPosition.BottomMiddle:
                case WatermarkPosition.BottomRight:
                default:
                    rectangle = new Rectangle(0, imageHeight - backgroundHeight - margin, imageWidth, backgroundHeight);
                    break;
            }

            return rectangle;
        }

        private static int GetTextAlign(SizeF textMetrics, int imageWidth, WatermarkPosition watermarkPosition)
        {
            int space;
            switch (watermarkPosition)
            {
                case WatermarkPosition.TopLeft:
                case WatermarkPosition.MiddleLeft:
                case WatermarkPosition.BottomLeft:
                default:
                    space = 5;
                    break;

                case WatermarkPosition.TopMiddle:
                case WatermarkPosition.Center:
                case WatermarkPosition.BottomMiddle:
                    space = (int)(imageWidth - textMetrics.Width) / 2;
                    break;

                case WatermarkPosition.TopRight:
                case WatermarkPosition.MiddleRight:
                case WatermarkPosition.BottomRight:
                    space = (int)(imageWidth - textMetrics.Width) - 5;
                    break;
            }

            return space;
        }
    }
}
