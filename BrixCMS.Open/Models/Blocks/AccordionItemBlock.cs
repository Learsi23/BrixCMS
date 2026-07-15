using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Accordion Item", Category = "Interactive", Icon = "fas fa-chevron-down",
        Description = "A single accordion item (question + answer). Use inside an Accordion block.", Child = true)]
    public class AccordionItemBlock : BlockBase
    {
        [Field(Title = "Pregunta", Placeholder = "Escribe la pregunta")]
        public StringField Question { get; set; } = new();

        [Field(Title = "Respuesta (puede incluir HTML)")]
        public TextAreaField Answer { get; set; } = new() { Rows = 4 };

        [Field(Title = "Abierto por defecto")]
        public BoolField OpenByDefault { get; set; } = new() { Value = "false" };
    }
}
