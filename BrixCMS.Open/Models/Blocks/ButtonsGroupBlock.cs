using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Buttons Group", Category = "Interactive", Icon = "fas fa-grip-lines",
        Description = "A row of one or more buttons, each fully stylable on its own. Add Button items inside — great for CTAs with multiple actions.")]
    public class ButtonsGroupBlock : BlockGroupBase
    {
        [Field(Title = "Alignment", Placeholder = "left | center | right")]
        public StringField Alignment { get; set; } = new() { Value = "center" };

        [Field(Title = "Gap Between Buttons", Description = "CSS gap value, e.g. 1rem.")]
        public StringField Gap { get; set; } = new() { Value = "1rem" };
    }
}
