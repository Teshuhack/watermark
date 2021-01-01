using System.Drawing;
using Watermark.Enums;

namespace Watermark.Models
{
    public class TextWatermarkOptions
    {
        public Color TextColor { get; set; } = Color.FromArgb(255, Color.White);

        public int FontSize { get; set; } = 24;

        public FontStyle FontStyle { get; set; } = FontStyle.Regular;

        public string FontName { get; set; } = "Arial";

        public Color BackgrounColor { get; set; } = Color.FromArgb(0, Color.White);

        public int Margin { get; set; } = -5;

        public WatermarkPosition WatermarkPosition { get; set; } = WatermarkPosition.BottomRight;

        public Color OutlineColor { get; set; } = Color.FromArgb(255, Color.Black);

        public float OutlineWidth { get; set; } = 3.5f;
    }
}
