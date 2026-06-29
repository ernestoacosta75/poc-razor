using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

        [HtmlAttributeName("columns")]
        public object? GridColumns { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.Columns = this.GridColumns;

            await base.ProcessAsync(context, output);
        }
    }
}
