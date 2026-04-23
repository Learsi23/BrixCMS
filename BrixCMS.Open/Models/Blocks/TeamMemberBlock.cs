using BrixCMS.Open.Models.Base;
using BrixCMS.Open.Data.Fields;

namespace BrixCMS.Open.Models.Blocks
{
    [BlockType(Name = "Team Member", Category = "Content", Icon = "fas fa-user-tie",
        Description = "Profile of a team member. Use within the Team block.")]
    public class TeamMemberBlock : BlockBase
    {
        [Field(Title = "Photo", Description = "Photo of the team member.")]
        public ImageField Photo { get; set; } = new();

        [Field(Title = "Full Name", Description = "Full name of the team member.")]
        public StringField Name { get; set; } = new();

        [Field(Title = "Role / Position", Description = "Role or position of the team member.")]
        public StringField Role { get; set; } = new();

        [Field(Title = "Brief Description", Description = "Brief description of the team member.")]
        public TextAreaField Description { get; set; } = new() { Rows = 3 };

        [Field(Title = "LinkedIn URL", Description = "URL to the team member's LinkedIn profile.")]
        public StringField LinkedInUrl { get; set; } = new();

        [Field(Title = "Twitter/X URL", Description = "URL to the team member's Twitter/X profile.")]
        public StringField TwitterUrl { get; set; } = new();

        [Field(Title = "Email", Description = "Email address of the team member.")]
        public StringField Email { get; set; } = new();

        [Field(Title = "Card Background Color", Description = "Background color of the member card.")]
        public ColorField CardBgColor { get; set; } = new() { Value = "#ffffff" };
    }
}
