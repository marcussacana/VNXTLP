using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

public static class BRSynonymous {
    public static Result[] DownloadResults(string Word) {
        List<Result> Results = new List<Result>();
        WebClient Client = new WebClient();
        Client.Encoding = Encoding.GetEncoding("iso-8859-1");
        string HTML;
        try {
            HTML = Client.DownloadString($"https://www.sinonimos.com.br/{DelAccents(Word).Trim()}");
        } catch { return new Result[0]; }

        string[] Parts = StringSplit(HTML, "<div class='s-wrapper'>");
        for (int x = 1; x < Parts.Length; x++) {
            string[] Divs = StringSplit(Parts[x], "</div>", false);
            Result Result = new Result();
            const string MeaningPrefix = "<div class=\"sentido\">";
            int Index = 0;
            if (Divs[0].Contains(MeaningPrefix)) {
                Result.Meaning = Divs[0].Substring(Divs[0].IndexOf(MeaningPrefix) + MeaningPrefix.Length).Trim(' ', ':');
                Index++;
            }
            Divs = StringSplit(Divs[Index].Substring(Divs[Index].IndexOf("<a href=")), "</a>");
            const string SynonymousPrefix = "class=\"sinonimo\">";
            List<string> Words = new List<string>();
            for (int i = 0; i < Divs.Length; i++) {
                if (!Divs[i].Contains(SynonymousPrefix))
                    continue;
                string SynWord = Divs[i].Substring(Divs[i].IndexOf(SynonymousPrefix) + SynonymousPrefix.Length);
                Words.Add(SynWord);
            }
            Result.Synonymous = Words.ToArray();
            Results.Add(Result);
        }

        return Results.ToArray();
    }

    public static string SearchWord(string Word) {
        var Results = DownloadResults(Word);
        if (Results.Length == 0)
            return string.Empty;

        string Text = string.Empty;
        foreach (var Result in Results) {
            bool HaveMeaning = !string.IsNullOrEmpty(Result.Meaning);
            if (HaveMeaning)
                Text += Result.Meaning + "\r\n-";
            else
                Text += "-";
            foreach (string SynWord in Result.Synonymous) {
                Text += $"{SynWord}, ";
            }
            Text = Text.Substring(0, Text.Length - 2);
            Text += "\r\n\r\n";
        }

        return Text.Substring(0, Text.Length - 4);
    }
    private static string[] StringSplit(string String, string Match, bool CutSplitter = false) {
        List<string> Parts = new List<string>();
        int Skip = 0;
        int IO = 0;
        while ((IO = String.IndexOf(Match, Skip)) != -1) {
            Parts.Add(String.Substring(0, IO));
            String = String.Substring(CutSplitter ? IO + Match.Length : IO);
            if (!CutSplitter)
                Skip = Match.Length;
        }
        if (String != string.Empty)
            Parts.Add(String);

        return Parts.ToArray();
    }

    private static string DelAccents(string Word) {
        Encoding Encoding = Encoding.GetEncoding(932);
        return Encoding.GetString(Encoding.GetBytes(Word));
    }
}

public struct Result {
    public string Meaning;

    public string[] Synonymous;
}

