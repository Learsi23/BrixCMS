namespace BrixCMS.Open.Services.Ingestion;

public static class ChunkingUtility
{
    public static List<string> SplitIntoChunks(string text, int maxWords = 80, int maxChars = 500)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= maxWords && text.Length <= maxChars) return [text];

        if (words.Length == 1)
        {
            var chunks = new List<string>();
            for (int i = 0; i < text.Length; i += maxChars)
            {
                var len = Math.Min(maxChars, text.Length - i);
                chunks.Add(text.Substring(i, len));
            }
            return chunks;
        }

        var result = new List<string>();
        var sb = new System.Text.StringBuilder();
        int count = 0;
        foreach (var word in words)
        {
            if ((count > 0 && count >= maxWords) || (sb.Length + word.Length + 1 > maxChars && count > 0))
            {
                result.Add(sb.ToString().TrimEnd());
                sb.Clear();
                count = 0;
            }
            sb.Append(word).Append(' ');
            count++;
        }
        if (sb.Length > 0) result.Add(sb.ToString().TrimEnd());
        return result;
    }
}
