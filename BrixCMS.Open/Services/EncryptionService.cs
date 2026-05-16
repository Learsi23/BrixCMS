using System.Security.Cryptography;
using System.Text;

namespace BrixCMS.Open.Services;

public class EncryptionService
{
    private readonly byte[] _key;

    public EncryptionService(IConfiguration config)
    {
        var keyStr = config["EncryptionKey"];
        if (string.IsNullOrWhiteSpace(keyStr))
            throw new InvalidOperationException(
                "EncryptionKey is not configured. Set it in appsettings.json or user-secrets before starting.");
        _key = SHA256.HashData(Encoding.UTF8.GetBytes(keyStr));
    }

    public (string EncryptedKey, string Iv, string AuthTag) Encrypt(string plaintext)
    {
        var iv = RandomNumberGenerator.GetBytes(12);
        var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
        var ciphertext = new byte[plaintextBytes.Length];
        var tag = new byte[16];

        using var aes = new AesGcm(_key, 16);
        aes.Encrypt(iv, plaintextBytes, ciphertext, tag);

        return (
            Convert.ToHexString(ciphertext).ToLowerInvariant(),
            Convert.ToHexString(iv).ToLowerInvariant(),
            Convert.ToHexString(tag).ToLowerInvariant()
        );
    }

    public string Decrypt(string encryptedHex, string ivHex, string authTagHex)
    {
        var ciphertext = Convert.FromHexString(encryptedHex);
        var iv         = Convert.FromHexString(ivHex);
        var tag        = Convert.FromHexString(authTagHex);
        var plaintext  = new byte[ciphertext.Length];

        using var aes = new AesGcm(_key, 16);
        aes.Decrypt(iv, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }

    public string MaskKey(string key)
    {
        if (key.Length <= 8) return "****";
        return key[..4] + "..." + key[^4..];
    }
}
