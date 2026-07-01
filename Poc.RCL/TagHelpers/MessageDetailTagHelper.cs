using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Poc.RCL.Components;

namespace Poc.RCL.TagHelpers
{
    [HtmlTargetElement("message-detail", TagStructure = TagStructure.WithoutEndTag)]
    public class MessageDetailTagHelper : BaseComponentTagHelper<MessageDetail>
    {
        public MessageDetailTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper)
        {
        }
    }
}
