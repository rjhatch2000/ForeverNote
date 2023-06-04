using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ForeverNote.Web.Framework.TagHelpers
{
    [HtmlTargetElement("forever-note-required")]
    public class RequiredTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "required");
            output.Content.SetContent("*");
        }
    }
}