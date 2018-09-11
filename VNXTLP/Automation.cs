using System.Drawing;
using System.Windows.Forms;

namespace VNXTLP {
    internal static partial class Engine {
        public static string ReloadString(string Dialogue) {
            LastString = Dialogue;
            if (Database.ContainsKey(Dialogue))
                return Database[Dialogue];

            return Dialogue;
        }

        public static bool CanReload(string Dialogue) {
            return Database.ContainsKey(Dialogue);
        }
        
        public static void FinishString(string Output) {
            if (!Database.ContainsKey(LastString))
                Database.Add(LastString, Output);
        }

        public static void HalfMaxmize(bool Left) {
            var Region = Screen.PrimaryScreen.WorkingArea;
            if (Left) {
                MainForm.Location = new Point(Region.Location.X + (Region.Width/2), Region.Y);
                MainForm.Size = new Size(Region.Size.Width/2, Region.Size.Height);
            } else {
                MainForm.Location = new Point(Region.Location.X, Region.Y);
                MainForm.Size = new Size(Region.Size.Width/2, Region.Size.Height);
            }
        }
    }
}
