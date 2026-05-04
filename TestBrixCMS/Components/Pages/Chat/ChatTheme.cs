namespace TestBrixCMS.Components.Pages.Chat;

public class ChatTheme
{
    public string UserBubbleColor { get; set; } = "#6366F1";
    public string UserTextColor { get; set; } = "#FFFFFF";
    public string AiBubbleColor { get; set; } = "#FFFFFF";
    public string AiTextColor { get; set; } = "#1E293B";
    public string AiBubbleBorderColor { get; set; } = "#E2E8F0";
    public string InputBgColor { get; set; } = "#FFFFFF";
    public string InputTextColor { get; set; } = "#0F172A";
    public string InputBorderColor { get; set; } = "#CBD5E1";
    public string InputPlaceholderColor { get; set; } = "#94A3B8";
    public string SendButtonColor { get; set; } = "#6366F1";
    public string ChatBgColor { get; set; } = "#F8FAFC";
    public string? AiLogo { get; set; }
    public string AiLogoSize { get; set; } = "28px";

    public static ChatTheme Default => new();

    public static ChatTheme Dark => new()
    {
        UserBubbleColor = "#6366F1",
        UserTextColor = "#FFFFFF",
        AiBubbleColor = "#1E293B",
        AiTextColor = "#E2E8F0",
        AiBubbleBorderColor = "#334155",
        InputBgColor = "#1E293B",
        InputTextColor = "#F8FAFC",
        InputBorderColor = "#334155",
        InputPlaceholderColor = "#64748B",
        SendButtonColor = "#6366F1",
        ChatBgColor = "#0F172A"
    };
}
