using System;
using System.Windows.Forms;
using System.Net; //No Import
using System.IO; //No Import
class Updater {
    internal int Major = 1;
    internal int Minor = 0;
    internal bool HaveUpdate(int MajorVersion, int MinorVersion) {
        if (Major > MajorVersion || Minor > MinorVersion)
            return true;
        return false;
    }

    internal bool GetUpdate(string MainExecutablePath) {
        try {
            string BasePath = Path.GetDirectoryName(MainExecutablePath) + "\\";
            byte[] Executable = DownloadData("http://vnx.uvnworks.com/Client/KRKR_exe");
            byte[] Launcher = DownloadData("http://vnx.uvnworks.com/Client/EndUp_exe");
            byte[] DLL = DownloadData("http://vnx.uvnworks.com/Client/KRKR_dll");
            File.WriteAllBytes(MainExecutablePath + "-Updated.exe", Executable);
            File.WriteAllBytes(BasePath + "Launcher.exe", Launcher);
            File.WriteAllBytes(BasePath + "KrKrSceneManager.dll-Updated.dll", DLL);
        }
        catch { return true; }
        return false;
    }

    private byte[] DownloadData(string Url) {
        HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
        Request.UseDefaultCredentials = true;
        Request.Method = "GET";
        WebResponse Response = Request.GetResponse();
        byte[] FC = new byte[0];
        using (MemoryStream Data = new MemoryStream())
        using (Stream Reader = Response.GetResponseStream()) {
            byte[] Buffer = new byte[1024];
            int bytesRead;
            do {
                bytesRead = Reader.Read(Buffer, 0, Buffer.Length);
                Data.Write(Buffer, 0, bytesRead);
            } while (bytesRead > 0);
            FC = Data.ToArray();
        }
        return FC;
    }
    
}