using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KnR
{
    public static class TextPreview {
        private static Font KnRFont = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 22f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Bitmap GetPreview(string Text) {
            Text = Text.Replace("", "-");
            Image TextBar = new Bitmap(850, 130);
            Graphics Render = Graphics.FromImage(TextBar);
            Render.FillRectangle((new SolidBrush(Color.Black)), new Rectangle(0, 0, TextBar.Size.Width, TextBar.Size.Height));
            int CharHeight = (int)Render.MeasureString(" ", KnRFont).Height;
            Rectangle DialogRegion = new Rectangle(new Point(0, 5), TextBar.Size);
            string show = string.Empty;
            for (int i = 0; i < Text.Length; i++) {
                if (i == 55 || i == 55 * 2 || i == 55 * 3)
                    show += '\n';
                if (i >= 55 * 4)
                    break;
                show += Text[i];
            }
            Render.DrawString(show, KnRFont, Brushes.White, DialogRegion);
            Render.Dispose();
            return TextBar as Bitmap;
        }
    }
}
