using System.ComponentModel;
using System.Drawing.Drawing2D;
using TranslateWordsGui.GraphicsUtils;
using static System.Math;

namespace TranslateWordsGui
{
    [DesignerCategory("code")]
    public class MessageFlower : Label
    {
        private object @lock = new object();
        private List<string> Messages { get; set; } = new List<string>();

        private List<Color> Colors { get; set; } = new List<Color>();

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            try
            { 
                var screen = new Bitmap(this.Size.Width, this.Size.Height);

                Graphics g = Graphics.FromImage(screen);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                int drawMessageIndex;
                lock (@lock)
                { 
                    drawMessageIndex = Messages.Count-1;
                }
                for (var y = this.Size.Height; 
                        y >= 0 && drawMessageIndex >= 0; ) 
                {
                    List<string> lines;
                    Color color;
                    lock (@lock)
                    { 
                        lines = g.WrapLines(Messages[drawMessageIndex], Font, this.Size.Width);
                        color = Colors[drawMessageIndex];
                    }
                    const int margin = 4;
                    foreach (var line in lines)
                    {
                        var fontHeight = g.MeasureString(line, Font);
                        var brush = new SolidBrush(color);
                        var textTop = y - (fontHeight.Height + margin);
                        g.DrawString(line, Font, brush, 0+margin, textTop);
                        y = (int)Ceiling(textTop);
                    }
                    drawMessageIndex--;
                    if (y < 0)
                        break;
                }
                g.Flush();

                e.Graphics.DrawImage(screen, 0, 0);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void AddParagraph(string translation, Color foreColor)
        {
            lock (@lock)
            { 
                Messages.Add(translation);
                Colors.Add(foreColor);
            }
            Invalidate();
        }
    }
}
