using PopupControl;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace VNXTLP {
    internal partial class BallonToolTip : UserControl {
        internal BallonToolTip() {
            InitializeComponent();
        }

        internal string Title {
            get { return lblTitle.Text; }
            set
            {
                lblTitle.Text = value;
                if (lblTitle.Location.X + lblTitle.Size.Width + 3 > Size.Width)
                    Size = new System.Drawing.Size(lblTitle.Location.X + lblTitle.Size.Width + 3, Size.Height);
            }
        }
        internal string Message {
            get { return LblMessage.Text; }
            set
            {
                LblMessage.Text = CompileText(value);
                int MinWidht = LblMessage.Location.X + LblMessage.Size.Width + 3;
                int MinHeight = LblMessage.Location.Y + LblMessage.Size.Height + 3;
                if (MinWidht > Size.Width)
                    Size = new System.Drawing.Size(MinWidht, Size.Height);
                if (MinHeight > Size.Height)
                    Size = new System.Drawing.Size(Size.Width, MinHeight);
            }
        }
        

        internal bool WordWrap = true;
        internal uint MaxWidht = 1000;
        private string CompileText(string cnt) {
            string Result = string.Empty;
            for (int i = 0; i < cnt.Length; i++) {
                if (Engine.TextWidth(Result + cnt[i], lblTitle.Font) > MaxWidht) {
                again:;
                    if (WordWrap) {
                        int NI = Result.Length;
                        while (NI > 0 && Result[--NI] != ' ')
                            if (Result[NI] != '\n')
                                continue;
                            else
                                NI = 0;
                        if (NI == 0) {
                            WordWrap = false;
                            goto again;
                        }
                        Result = Result.Substring(0, Result.Length - (i - NI));
                        i = NI;
                        Result += '\n';
                    } else {
                        i--;
                        Result += '\n';
                    }
                } else {
                    Result += cnt[i];
                }
            }
            return Result;
        }
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
        }
    }
}
