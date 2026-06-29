
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Poc.RCL.TagHelpers
{
    public abstract class BaseComponentTagHelper<TComponent> : TagHelper where TComponent : IComponent
    {
        protected readonly IHtmlHelper HtmlHelper;

        protected BaseComponentTagHelper(IHtmlHelper htmlHelper)
        {
            HtmlHelper = htmlHelper;
        }

        [HtmlAttributeName("data")]
        public object? Data { get; set; }

        [HtmlAttributeName("columns")]
        public object? Columns { get; set; }

        // Il decorator [ViewContext] dice esplicitamente a Razor di auto-popolare questa property.
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is not null)
            {
                (HtmlHelper as IViewContextAware)?.Contextualize(ViewContext);
            }

            var dictionaryParameters = new Dictionary<string, object?>();

            if (Data != null)
            {
                dictionaryParameters.Add("Data", Data);
            }

            if (Columns != null)
            {
                dictionaryParameters.Add("Columns", Columns);
            }

            if (this is MessagesGridTagHelper gridGridTagHelper && !string.IsNullOrEmpty(gridGridTagHelper.FilterUrl))
            {
                dictionaryParameters.Add("FilterUrl", gridGridTagHelper.FilterUrl);
            }

            var content = await HtmlHelper.RenderComponentAsync<TComponent>(RenderMode.Static, dictionaryParameters);

            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }
    }
}
