using BrixCMS.Open.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BrixCMS.Open.Services;

/// <summary>
/// Handles admin authentication: password hashing (PBKDF2),
/// TOTP 2FA (RFC 6238), and session management.
/// </summary>
public class AdminAuthService
{
    private readonly BrixDbContext _db;

    public AdminAuthService(BrixDbContext db) => _db = db;

    // ─────────────────────────────────────────────────────────────
    //  PASSWORD  (PBKDF2-SHA256, 100k iterations)
    // ─────────────────────────────────────────────────────────────

    public static string HashPassword(string password, out string salt)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(32);
        salt = Convert.ToBase64String(saltBytes);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, 100_000, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(hash);
    }

    public static bool VerifyPassword(string password, string hash, string salt)
    {
        try
        {
            var saltBytes  = Convert.FromBase64String(salt);
            var hashBytes  = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, 100_000, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(hash),
                hashBytes);
        }
        catch { return false; }
    }

    // ─────────────────────────────────────────────────────────────
    //  TOTP  (RFC 6238 — compatible with Google Authenticator)
    // ─────────────────────────────────────────────────────────────

    /// <summary>Generates a new Base32 secret for the admin.</summary>
    public static string GenerateTotpSecret()
    {
        var bytes = RandomNumberGenerator.GetBytes(20);
        return Base32Encode(bytes);
    }

    /// <summary>Returns the otpauth:// URL to display as a QR code.</summary>
    public static string GetOtpAuthUrl(string secret, string email, string issuer = "BrixCMS")
    {
        var encodedIssuer = Uri.EscapeDataString(issuer);
        var encodedEmail  = Uri.EscapeDataString(email);
        return $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={secret}&issuer={encodedIssuer}&algorithm=SHA1&digits=6&period=30";
    }

    /// </summary>/// <summary>Verifies a TOTP code (±1 tolerance window).</summary>
    public static bool VerifyTotp(string secret, string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 6) return false;
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30;
        for (long offset = -1; offset <= 1; offset++)
            if (GetTotpCode(secret, now + offset) == code.Trim())
                return true;
        return false;
    }

    private static string GetTotpCode(string secret, long counter)
    {
        var key = Base32Decode(secret);
        var counterBytes = BitConverter.GetBytes(counter);
        if (BitConverter.IsLittleEndian) Array.Reverse(counterBytes);
        using var hmac = new HMACSHA1(key);
        var hash   = hmac.ComputeHash(counterBytes);
        var offset = hash[^1] & 0x0f;
        var otp    = ((hash[offset]     & 0x7f) << 24)
                   | ((hash[offset + 1] & 0xff) << 16)
                   | ((hash[offset + 2] & 0xff) << 8)
                   |  (hash[offset + 3] & 0xff);
        return (otp % 1_000_000).ToString("D6");
    }

    // ─────────────────────────────────────────────────────────────
    //  BASE32  (alphabet RFC 4648)
    // ─────────────────────────────────────────────────────────────

    private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

    private static string Base32Encode(byte[] data)
    {
        var sb = new StringBuilder();
        int buffer = data[0], next = 1, bitsLeft = 8;
        while (bitsLeft > 0 || next < data.Length)
        {
            if (bitsLeft < 5)
            {
                if (next < data.Length)
                {
                    buffer <<= 8;
                    buffer |= data[next++] & 0xff;
                    bitsLeft += 8;
                }
                else
                {
                    int pad = 5 - bitsLeft;
                    buffer <<= pad;
                    bitsLeft += pad;
                }
            }
            int index = 0x1f & (buffer >> (bitsLeft - 5));
            bitsLeft -= 5;
            sb.Append(Base32Alphabet[index]);
        }
        return sb.ToString();
    }

    private static byte[] Base32Decode(string input)
    {
        input = input.ToUpperInvariant().TrimEnd('=');
        var output = new byte[input.Length * 5 / 8];
        int buffer = 0, bitsLeft = 0, idx = 0;
        foreach (char c in input)
        {
            int val = Base32Alphabet.IndexOf(c);
            if (val < 0) continue;
            buffer <<= 5;
            buffer |= val;
            bitsLeft += 5;
            if (bitsLeft >= 8)
            {
                output[idx++] = (byte)(buffer >> (bitsLeft - 8));
                bitsLeft -= 8;
            }
        }
        return output;
    }

    // ─────────────────────────────────────────────────────────────
    //  DB HELPERS
    // ─────────────────────────────────────────────────────────────

    public AdminUser? GetAdmin() => _db.AdminUsers.FirstOrDefault();

    public async Task<AdminUser?> GetAdminAsync() =>
        await _db.AdminUsers.FirstOrDefaultAsync();

    public async Task<AdminUser?> GetAdminByEmailAsync(string email) =>
        await _db.AdminUsers.FirstOrDefaultAsync(u =>
            u.Email.ToLower() == email.ToLower());

    public async Task<AdminUser?> GetAdminByIdAsync(int id) =>
        await _db.AdminUsers.FindAsync(id);

    public async Task<List<AdminUser>> GetAllAdminsAsync() =>
        await _db.AdminUsers.OrderBy(u => u.Id).ToListAsync();

    public async Task<AdminUser> CreateAdminAsync(string email, string name, string password)
    {
        var hash = HashPassword(password, out var salt);
        var user = new AdminUser
        {
            Email        = email,
            Name         = string.IsNullOrWhiteSpace(name) ? email : name,
            PasswordHash = hash,
            PasswordSalt = salt,
            IsOwner      = false,
            CreatedAt    = DateTime.UtcNow
        };
        _db.AdminUsers.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAdminAsync(int id)
    {
        var user = await _db.AdminUsers.FindAsync(id);
        if (user == null || user.IsOwner) return false;
        _db.AdminUsers.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task UpdateEmailAsync(int id, string newEmail)
    {
        var u = await _db.AdminUsers.FindAsync(id);
        if (u == null) return;
        u.Email = newEmail;
        await _db.SaveChangesAsync();
    }

    public async Task UpdatePasswordAsync(int id, string newPassword)
    {
        var u = await _db.AdminUsers.FindAsync(id);
        if (u == null) return;
        u.PasswordHash = HashPassword(newPassword, out var salt);
        u.PasswordSalt = salt;
        await _db.SaveChangesAsync();
    }

    public async Task SetTwoFactorAsync(int id, bool enabled, string? secret = null)
    {
        var u = await _db.AdminUsers.FindAsync(id);
        if (u == null) return;
        u.TwoFactorEnabled = enabled;
        if (secret != null) u.TwoFactorSecret = secret;
        await _db.SaveChangesAsync();
    }
}
