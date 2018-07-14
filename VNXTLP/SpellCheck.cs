using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace VNXTLP {
    public abstract class Hunspell : IDisposable {
        public static Hunspell GetHunspell(string dictionary) {
            if (Info.IsRunningOnLinux() || Engine.GetConfig("VNXTLP", "System", false).ToLower() == "linux")
                return new LinuxHunspell(dictionary + ".aff", dictionary + ".dic");
            if (Info.IsRunningOnMac() || Engine.GetConfig("VNXTLP", "System", false).ToLower() == "mac")
                return new MacHunspell(dictionary + ".aff", dictionary + ".dic");

            // Finnish uses Voikko (not available via hunspell)
            if (dictionary.EndsWith("fi_fi", StringComparison.OrdinalIgnoreCase))
                return new VoikkoSpellCheck(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.BaseDirectory);

            return new WindowsHunspell(dictionary + ".aff", dictionary + ".dic");
        }

        public abstract bool Spell(string word);
        public abstract List<string> Suggest(string word);

        public virtual void Dispose() {
        }

        protected void AddIShouldBeLowercaseLSuggestion(List<string> suggestions, string word) {
            if (suggestions == null) {
                return;
            }

            // "I" can often be an ocr bug - should really be "l"
            if (word.Length > 1 && word.StartsWith("I") && !suggestions.Contains("l" + word.Substring(1)) && Spell("l" + word.Substring(1))) {
                suggestions.Add("l" + word.Substring(1));
            }
        }

    }
    public class WindowsHunspell : Hunspell {
        private NHunspell.Hunspell _hunspell;

        public WindowsHunspell(string affDictionary, string dicDictionary) {
            _hunspell = new NHunspell.Hunspell(affDictionary, dicDictionary);
        }

        public override bool Spell(string word) {
            return _hunspell.Spell(word);
        }

        public override List<string> Suggest(string word) {
            var list = _hunspell.Suggest(word);
            AddIShouldBeLowercaseLSuggestion(list, word);
            return list;
        }

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                if (_hunspell != null && !_hunspell.IsDisposed)
                    _hunspell.Dispose();
                _hunspell = null;
            }
        }

    }
    public class VoikkoSpellCheck : Hunspell {

        // Voikko functions in dll
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr VoikkoInit(ref IntPtr error, byte[] languageCode, byte[] path);
        private VoikkoInit _voikkoInit;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void VoikkoTerminate(IntPtr libVlc);
        private VoikkoTerminate _voikkoTerminate;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 VoikkoSpell(IntPtr handle, byte[] word);
        private VoikkoSpell _voikkoSpell;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr VoikkoSuggest(IntPtr handle, byte[] word);
        private VoikkoSuggest _voikkoSuggest;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr VoikkoFreeCstrArray(IntPtr array);
        private VoikkoFreeCstrArray _voikkoFreeCstrArray;

        private IntPtr _libDll = IntPtr.Zero;
        private IntPtr _libVoikko = IntPtr.Zero;

        private static string N2S(IntPtr ptr) {
            if (ptr == IntPtr.Zero)
                return null;
            List<byte> bytes = new List<byte>();

            while (Marshal.ReadByte(ptr, bytes.Count) != 0)
                bytes.Add(Marshal.ReadByte(ptr, bytes.Count));

            return N2S(bytes.ToArray());
        }

        private static string N2S(byte[] bytes) {
            if (bytes == null)
                return null;
            return Encoding.UTF8.GetString(bytes);
        }

        private static byte[] S2N(string str) {
            return S2Encoding(str, Encoding.UTF8);
        }

        private static byte[] S2Ansi(string str) {
            return S2Encoding(str, Encoding.Default);
        }

        private static byte[] S2Encoding(string str, Encoding encoding) {
            if (str == null)
                return null;
            return encoding.GetBytes(str + '\0');
        }

        private object GetDllType(Type type, string name) {
            IntPtr address = Unamanaged.GetProcAddress(_libDll, name);
            if (address != IntPtr.Zero) {
                return Marshal.GetDelegateForFunctionPointer(address, type);
            }
            return null;
        }

        /// <summary>
        /// Load dll dynamic + set pointers to needed methods
        /// </summary>
        /// <param name="baseFolder"></param>
        private void LoadLibVoikkoDynamic(string baseFolder) {
            string dllFile = Path.Combine(baseFolder, "Voikkox86.dll");
            if (IntPtr.Size == 8)
                dllFile = Path.Combine(baseFolder, "Voikkox64.dll");
            if (!File.Exists(dllFile))
                throw new FileNotFoundException(dllFile);
            _libDll = Unamanaged.LoadLibrary(dllFile);
            if (_libDll == IntPtr.Zero)
                throw new FileLoadException("Unable to load " + dllFile);

            _voikkoInit = (VoikkoInit)GetDllType(typeof(VoikkoInit), "voikkoInit");
            _voikkoTerminate = (VoikkoTerminate)GetDllType(typeof(VoikkoTerminate), "voikkoTerminate");
            _voikkoSpell = (VoikkoSpell)GetDllType(typeof(VoikkoSpell), "voikkoSpellCstr");
            _voikkoSuggest = (VoikkoSuggest)GetDllType(typeof(VoikkoSuggest), "voikkoSuggestCstr");
            _voikkoFreeCstrArray = (VoikkoFreeCstrArray)GetDllType(typeof(VoikkoFreeCstrArray), "voikkoFreeCstrArray");

            if (_voikkoInit == null || _voikkoTerminate == null || _voikkoSpell == null || _voikkoSuggest == null || _voikkoFreeCstrArray == null)
                throw new FileLoadException("Not all methods in Voikko dll could be found!");
        }

        public override bool Spell(string word) {
            if (string.IsNullOrEmpty(word))
                return false;

            return Convert.ToBoolean(_voikkoSpell(_libVoikko, S2N(word)));
        }

        public override List<string> Suggest(string word) {
            var suggestions = new List<string>();
            if (string.IsNullOrEmpty(word))
                return suggestions;
            IntPtr voikkoSuggestCstr = _voikkoSuggest(_libVoikko, S2N(word));
            if (voikkoSuggestCstr == IntPtr.Zero)
                return suggestions;

            /* 
            unsafe {
                for (byte** cStr = (byte**)voikkoSuggestCstr; *cStr != (byte*)0; cStr++)
                    suggestions.Add(N2S(new IntPtr(*cStr)));
            }*/

            System.Diagnostics.Debug.Write("UNTESTED CODE"); //This code is a safe version of the commented code above
            while (ReadPointer(voikkoSuggestCstr, suggestions.Count) != 0) {
                suggestions.Add(N2S(new IntPtr(ReadPointer(voikkoSuggestCstr, suggestions.Count))));
            }

            _voikkoFreeCstrArray(voikkoSuggestCstr);
            return suggestions;
        }

        private dynamic ReadPointer(IntPtr Pointer, int Skip) => Environment.Is64BitProcess ? Marshal.ReadInt64(Pointer, Skip * 8) : Marshal.ReadInt32(Pointer, Skip * 4);

        public VoikkoSpellCheck(string baseFolder, string dictionaryFolder) {
            LoadLibVoikkoDynamic(baseFolder);

            var error = new IntPtr();
            _libVoikko = _voikkoInit(ref error, S2N("fi"), S2Ansi(dictionaryFolder));
            if (_libVoikko == IntPtr.Zero && error != IntPtr.Zero)
                throw new Exception(N2S(error));
        }

        ~VoikkoSpellCheck() {
            Dispose(false);
        }

        private void ReleaseUnmangedResources() {
            try {
                if (_libVoikko != IntPtr.Zero) {
                    _voikkoTerminate(_libVoikko);
                    _libVoikko = IntPtr.Zero;
                }

                if (_libDll != IntPtr.Zero) {
                    Unamanaged.FreeLibrary(_libDll);
                    _libDll = IntPtr.Zero;
                }
            } catch {
            }
        }

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                //ReleaseManagedResources();
            }
            ReleaseUnmangedResources();
        }

    }
    public class MacHunspell : Hunspell {

        private IntPtr _hunspellHandle = IntPtr.Zero;

        public MacHunspell(string affDirectory, string dicDictory) {
            //Also search - /usr/share/hunspell
            _hunspellHandle = Unamanaged.Hunspell_create(affDirectory, dicDictory);
        }

        public override bool Spell(string word) {
            return Unamanaged.Hunspell_spell(_hunspellHandle, word) != 0;
        }

        public override List<string> Suggest(string word) {
            IntPtr pointerToAddressStringArray = Marshal.AllocHGlobal(IntPtr.Size);
            int resultCount = Unamanaged.Hunspell_suggest(_hunspellHandle, pointerToAddressStringArray, word);
            IntPtr addressStringArray = Marshal.ReadIntPtr(pointerToAddressStringArray);
            List<string> results = new List<string>();
            for (int i = 0; i < resultCount; i++) {
                IntPtr addressCharArray = Marshal.ReadIntPtr(addressStringArray, i * IntPtr.Size);
                string suggestion = Marshal.PtrToStringAuto(addressCharArray);
                if (string.IsNullOrEmpty(suggestion))
                    results.Add(suggestion);
            }
            Unamanaged.Hunspell_free_list(_hunspellHandle, pointerToAddressStringArray, resultCount);
            Marshal.FreeHGlobal(pointerToAddressStringArray);

            return results;
        }

        ~MacHunspell() {
            Dispose(false);
        }

        private void ReleaseUnmangedResources() {
            if (_hunspellHandle != IntPtr.Zero) {
                Unamanaged.Hunspell_destroy(_hunspellHandle);
                _hunspellHandle = IntPtr.Zero;
            }
        }

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                //ReleaseManagedResources();
            }
            ReleaseUnmangedResources();
        }

    }
    public class LinuxHunspell : Hunspell {
        private IntPtr _hunspellHandle = IntPtr.Zero;

        public LinuxHunspell(string affDirectory, string dicDictory) {
            //Also search - /usr/share/hunspell
            try {
                _hunspellHandle = Unamanaged.Hunspell_create(affDirectory, dicDictory);
            } catch {
                System.Windows.Forms.MessageBox.Show("Unable to start hunspell spell checker - make sure hunspell is installed!");
                throw;
            }
        }

        public override bool Spell(string word) {
            return Unamanaged.Hunspell_spell(_hunspellHandle, word) != 0;
        }

        public override List<string> Suggest(string word) {
            IntPtr pointerToAddressStringArray = Marshal.AllocHGlobal(IntPtr.Size);
            int resultCount = Unamanaged.Hunspell_suggest(_hunspellHandle, pointerToAddressStringArray, word);
            IntPtr addressStringArray = Marshal.ReadIntPtr(pointerToAddressStringArray);
            List<string> results = new List<string>();
            for (int i = 0; i < resultCount; i++) {
                IntPtr addressCharArray = Marshal.ReadIntPtr(addressStringArray, i * IntPtr.Size);
                string suggestion = Marshal.PtrToStringAuto(addressCharArray);
                if (!string.IsNullOrEmpty(suggestion))
                    results.Add(suggestion);
            }
            Unamanaged.Hunspell_free_list(_hunspellHandle, pointerToAddressStringArray, resultCount);
            Marshal.FreeHGlobal(pointerToAddressStringArray);

            return results;
        }

        ~LinuxHunspell() {
            Dispose(false);
        }

        private void ReleaseUnmangedResources() {
            if (_hunspellHandle != IntPtr.Zero) {
                Unamanaged.Hunspell_destroy(_hunspellHandle);
                _hunspellHandle = IntPtr.Zero;
            }
        }

        public override void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                //ReleaseManagedResources();
            }
            ReleaseUnmangedResources();
        }

    }

    public static class Info {
        public static bool IsRunningOnLinux() {
            return Environment.OSVersion.Platform == PlatformID.Unix && !IsRunningOnMac();
        }

        public static bool IsRunningOnMac() {
            return Environment.OSVersion.Platform == PlatformID.MacOSX ||
                (Environment.OSVersion.Platform == PlatformID.Unix &&
                 Directory.Exists("/Applications") &&
                 Directory.Exists("/System") &&
                 Directory.Exists("/Users"));
        }
    }

    public static class Unamanaged {
        #region Hunspell

        [DllImport("libhunspell", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern IntPtr Hunspell_create(string affpath, string dpath);

        [DllImport("libhunspell")]
        internal static extern IntPtr Hunspell_destroy(IntPtr hunspellHandle);

        [DllImport("libhunspell", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern int Hunspell_spell(IntPtr hunspellHandle, string word);

        [DllImport("libhunspell", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern int Hunspell_suggest(IntPtr hunspellHandle, IntPtr slst, string word);

        [DllImport("libhunspell")]
        internal static extern void Hunspell_free_list(IntPtr hunspellHandle, IntPtr slst, int n);

        #endregion Hunspell

        #region Win32 API

        // Win32 API functions for dynamically loading DLLs
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, BestFitMapping = false)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("user32.dll")]
        internal static extern short GetKeyState(int vKey);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FreeConsole();

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        internal static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int width, int height, int wFlags);

        #endregion Win32 API
    }

}