using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Poc.RCL.Components;

namespace Poc.RCL.TagHelpers
{
    [HtmlTargetElement("messages-grid", TagStructure = TagStructure.WithoutEndTag)]
    public class MessagesGridTagHelper : BaseComponentTagHelper<MessagesGrid>
    {
        public MessagesGridTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper)
        {
        }

        [HtmlAttributeName("filter-url")]
        public string FilterUrl { get; set; } = string.Empty;
    }
}
