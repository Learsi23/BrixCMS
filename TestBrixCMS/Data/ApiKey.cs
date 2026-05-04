namespace TestBrixCMS.Data;

public class ApiKey
{
    public int Id { get; set; }
    public string Provider { get; set; } = "";    // "gemini" | "deepseek" | "mistral"
    public string EncryptedKey { get; set; } = "";
    public string Iv { get; set; } = "";
    public string AuthTag { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
