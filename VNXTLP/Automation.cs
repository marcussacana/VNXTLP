namespace VNXTLP {
    internal static partial class Engine {
        public static string ReloadString(string Dialogue) {
            LastString = Dialogue;
            if (Database.ContainsKey(Dialogue))
                return Database[Dialogue];

            return Dialogue;
        }
        
        public static void FinishString(string Output) {
            if (!Database.ContainsKey(LastString))
                Database.Add(LastString, Output);
        }
    }
}
