using Microsoft.AspNetCore.Components;
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

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is not null)
            {
                (HtmlHelper as IViewContextAware)?.Contextualize(ViewContext);
            }

            var content = await HtmlHelper.RenderComponentAsync<MessagesGrid>(
                RenderMode.Static,
                new { Data, FilterUrl });

            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }
    }
}
