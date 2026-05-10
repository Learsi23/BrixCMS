using BrixCMS.Open.Services.Ingestion;

namespace BrixCMS.Open.Tests;

public class ChunkingUtilityTests
{
    [Fact]
    public void Short_text_returns_single_chunk()
    {
        var result = ChunkingUtility.SplitIntoChunks("Hello world", maxWords: 80, maxChars: 500);
        Assert.Single(result);
        Assert.Equal("Hello world", result[0]);
    }

    [Fact]
    public void Long_text_split_by_word_count()
    {
        var words = string.Join(" ", Enumerable.Range(0, 100).Select(i => $"word{i}"));
        var result = ChunkingUtility.SplitIntoChunks(words, maxWords: 30, maxChars: 9999);
        Assert.True(result.Count >= 3);
        foreach (var chunk in result)
            Assert.True(chunk.Split(' ').Length <= 30);
    }

    [Fact]
    public void Single_long_word_split_by_chars()
    {
        var longWord = new string('x', 1500);
        var result = ChunkingUtility.SplitIntoChunks(longWord, maxWords: 80, maxChars: 500);
        Assert.True(result.Count >= 3);
        foreach (var chunk in result)
            Assert.True(chunk.Length <= 500);
    }

    [Fact]
    public void Compact_json_split_by_chars()
    {
        var json = $"{{\"products\":[{string.Join(",", Enumerable.Range(0, 50).Select(i => $"{{\"id\":{i},\"name\":\"product{i}\"}}"))}]}}";
        var result = ChunkingUtility.SplitIntoChunks(json, maxWords: 80, maxChars: 500);
        Assert.True(result.Count >= 2);
        foreach (var chunk in result)
            Assert.True(chunk.Length <= 500);
    }

    [Fact]
    public void Empty_text_returns_single_chunk()
    {
        var result = ChunkingUtility.SplitIntoChunks("", maxWords: 80, maxChars: 500);
        Assert.Single(result);
        Assert.Equal("", result[0]);
    }

    [Fact]
    public void Mixed_word_and_char_limits_triggers_word_split_first()
    {
        var words = string.Join(" ", Enumerable.Range(0, 10).Select(i => new string('x', 100)));
        var result = ChunkingUtility.SplitIntoChunks(words, maxWords: 3, maxChars: 9999);
        Assert.True(result.Count >= 3);
    }
}
