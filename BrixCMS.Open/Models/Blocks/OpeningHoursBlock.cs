using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Opening Hours", Category = "Content", Icon = "fas fa-clock",
        Description = "Weekly opening hours table with a live open/closed status indicator.")]
    public class OpeningHoursBlock : BlockBase
    {
        [Header("Content")]
        [Field(Title = "Section Title", Placeholder = "Opening Hours")]
        public StringField Title { get; set; } = new() { Value = "Opening Hours" };

        [Field(Title = "Closed Label", Placeholder = "Closed")]
        public StringField ClosedLabel { get; set; } = new() { Value = "Closed" };

        [Field(Title = "Now Open Label", Placeholder = "We're open now!")]
        public StringField NowOpenLabel { get; set; } = new() { Value = "We're open now!" };

        [Field(Title = "Now Closed Label", Placeholder = "We're closed right now")]
        public StringField NowClosedLabel { get; set; } = new() { Value = "We're closed right now" };

        // ── Monday ──────────────────────────────────────────────────────────
        [Header("Monday")]
        [Field(Title = "Monday — Open", Placeholder = "09:00  (leave empty = closed)")]
        public StringField MondayOpen { get; set; } = new() { Value = "09:00" };

        [Field(Title = "Monday — Close", Placeholder = "17:00")]
        public StringField MondayClose { get; set; } = new() { Value = "17:00" };

        // ── Tuesday ─────────────────────────────────────────────────────────
        [Header("Tuesday")]
        [Field(Title = "Tuesday — Open", Placeholder = "09:00  (leave empty = closed)")]
        public StringField TuesdayOpen { get; set; } = new() { Value = "09:00" };

        [Field(Title = "Tuesday — Close", Placeholder = "17:00")]
        public StringField TuesdayClose { get; set; } = new() { Value = "17:00" };

        // ── Wednesday ───────────────────────────────────────────────────────
        [Header("Wednesday")]
        [Field(Title = "Wednesday — Open", Placeholder = "09:00  (leave empty = closed)")]
        public StringField WednesdayOpen { get; set; } = new() { Value = "09:00" };

        [Field(Title = "Wednesday — Close", Placeholder = "17:00")]
        public StringField WednesdayClose { get; set; } = new() { Value = "17:00" };

        // ── Thursday ────────────────────────────────────────────────────────
        [Header("Thursday")]
        [Field(Title = "Thursday — Open", Placeholder = "09:00  (leave empty = closed)")]
        public StringField ThursdayOpen { get; set; } = new() { Value = "09:00" };

        [Field(Title = "Thursday — Close", Placeholder = "17:00")]
        public StringField ThursdayClose { get; set; } = new() { Value = "17:00" };

        // ── Friday ──────────────────────────────────────────────────────────
        [Header("Friday")]
        [Field(Title = "Friday — Open", Placeholder = "09:00  (leave empty = closed)")]
        public StringField FridayOpen { get; set; } = new() { Value = "09:00" };

        [Field(Title = "Friday — Close", Placeholder = "15:00")]
        public StringField FridayClose { get; set; } = new() { Value = "15:00" };

        // ── Saturday ────────────────────────────────────────────────────────
        [Header("Saturday")]
        [Field(Title = "Saturday — Open", Placeholder = "10:00  (leave empty = closed)")]
        public StringField SaturdayOpen { get; set; } = new() { Value = "" };

        [Field(Title = "Saturday — Close", Placeholder = "14:00")]
        public StringField SaturdayClose { get; set; } = new() { Value = "" };

        // ── Sunday ──────────────────────────────────────────────────────────
        [Header("Sunday")]
        [Field(Title = "Sunday — Open", Placeholder = "leave empty = closed")]
        public StringField SundayOpen { get; set; } = new() { Value = "" };

        [Field(Title = "Sunday — Close", Placeholder = "leave empty = closed")]
        public StringField SundayClose { get; set; } = new() { Value = "" };

        // ── Style ────────────────────────────────────────────────────────────
        [Header("Style")]
        [Field(Title = "Open Badge Color")]
        public ColorField OpenColor { get; set; } = new() { Value = "#16a34a" };

        [Field(Title = "Closed Badge Color")]
        public ColorField ClosedColor { get; set; } = new() { Value = "#dc2626" };

        [Field(Title = "Highlight Today")]
        public BoolField HighlightToday { get; set; } = new() { Value = "true" };
    }
}
