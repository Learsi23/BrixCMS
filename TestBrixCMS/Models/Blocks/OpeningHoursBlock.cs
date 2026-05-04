using TestBrixCMS.Models.Base;
using TestBrixCMS.Data.Fields;

namespace TestBrixCMS.Models.Blocks
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

        [Header("Hours (JSON)")]
        [Field(Title = "Hours JSON",
            Description = "Array of { day, open, close } — use empty strings for closed days.",
            Placeholder = "[]")]
        public TextAreaField HoursJson { get; set; } = new()
        {
            Value = """
[
  { "day": "Monday",    "open": "09:00", "close": "17:00" },
  { "day": "Tuesday",   "open": "09:00", "close": "17:00" },
  { "day": "Wednesday", "open": "09:00", "close": "17:00" },
  { "day": "Thursday",  "open": "09:00", "close": "17:00" },
  { "day": "Friday",    "open": "09:00", "close": "15:00" },
  { "day": "Saturday",  "open": "",      "close": ""      },
  { "day": "Sunday",    "open": "",      "close": ""      }
]
"""
        };

        [Header("Style")]
        [Field(Title = "Open Badge Color")]
        public ColorField OpenColor { get; set; } = new() { Value = "#16a34a" };

        [Field(Title = "Closed Badge Color")]
        public ColorField ClosedColor { get; set; } = new() { Value = "#dc2626" };

        [Field(Title = "Highlight Today")]
        public BoolField HighlightToday { get; set; } = new() { Value = "true" };
    }
}
