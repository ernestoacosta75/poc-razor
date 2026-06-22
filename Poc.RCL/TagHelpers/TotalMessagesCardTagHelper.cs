using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Poc.RCL.Components;

namespace Poc.RCL.TagHelpers
{
    [HtmlTargetElement("total-messages-card", TagStructure = TagStructure.WithoutEndTag)]
    public class TotalMessagesCardTagHelper : BaseComponentTagHelper<TotalMessagesCard>
    {
        public TotalMessagesCardTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper)
        {
        }
    }
}
