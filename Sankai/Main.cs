using System.Drawing;

namespace Sankai {
    public static class TextPreview
    {
        private static Font SankaiFont = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 24f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Bitmap GetPreview(string text) {
            Image TextBar = Sankai.Properties.Resources.textbar;
            Graphics Render = Graphics.FromImage(TextBar);
            int CharHeight = (int)Render.MeasureString(" ", SankaiFont).Height;
            Rectangle Region1 = new Rectangle(new Point(85, 20), new Size(880, CharHeight));
            Rectangle Region2 = new Rectangle(new Point(85, 20 + ((CharHeight + (CharHeight / 4)) * 1)), new Size(880, CharHeight));
            Rectangle Region3 = new Rectangle(new Point(85, 20 + ((CharHeight + (CharHeight / 4)) * 2)), new Size(880, CharHeight));
            string One = "", Thow = "", Three = "";
            foreach (char c in text) {
                if (One.Length > 50) {
                    if (Thow.Length > 50) {
                        if (Three.Length > 50) {
                            continue;
                        } else
                            Three += c;
                    } else
                        Thow += c;
                } else
                    One += c;
            }
            Render.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Render.DrawString(One, SankaiFont, Brushes.White, Region1);
            Render.DrawString(Thow, SankaiFont, Brushes.White, Region2);
            Render.DrawString(Three, SankaiFont, Brushes.White, Region3);
            Render.Dispose();
            return TextBar as Bitmap;
        }
    }
    
}
