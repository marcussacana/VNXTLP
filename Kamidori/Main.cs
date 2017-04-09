using System.Drawing;

namespace Kamidori
{
    public static class TextPreview
    {
        private static Font KamidoriFont = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 22f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Bitmap GetPreview(string FirstLine, string SecondLine) {
            Image TextBar = Kamidori.Properties.Resources.textbar;
            Graphics Render = Graphics.FromImage(TextBar);
            int CharHeight = (int)Render.MeasureString(" ", KamidoriFont).Height;
            Rectangle DialogRegion1 = new Rectangle(new Point(85, 40), new Size(880, CharHeight));
            Rectangle DialogRegion2 = new Rectangle(new Point(85, 40 + (CharHeight + (CharHeight/3))), new Size(880, CharHeight));
            string FisrtLine = FirstLine.Substring(0, FirstLine.Length > 64 ? 64 : FirstLine.Length);
            if (FirstLine.Length > 64)
                SecondLine = FirstLine.Substring(64, FirstLine.Length - 64);
            Render.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Render.DrawString(FisrtLine, KamidoriFont, Brushes.White, DialogRegion1);
            Render.DrawString(SecondLine, KamidoriFont, Brushes.White, DialogRegion2);
            Render.Dispose();
            return TextBar as Bitmap;
        }
    }
    
}
