
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Poc.RCL.TagHelpers
{
    public abstract class BaseComponentTagHelper<TComponent> : TagHelper where TComponent : IComponent
    {
        private readonly IHtmlHelper _htmlHelper;

        protected BaseComponentTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        [HtmlAttributeName("data")]
        public object? Data { get; set; }

        // Il decorator [ViewContext] dice esplicitamente a Razor di auto-popolare questa property.
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext? ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext is not null)
            {
                (_htmlHelper as IViewContextAware)?.Contextualize(ViewContext);
            }

            // Eseguendo il rendereing del componente
            var content = await _htmlHelper.RenderComponentAsync<TComponent>(RenderMode.Static, new { Data });

            output.TagName = null;
            output.Content.SetHtmlContent(content);
        }
    }
}
