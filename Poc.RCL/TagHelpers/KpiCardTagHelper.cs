using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Poc.RCL.Components;

namespace Poc.RCL.TagHelpers
{
    // This decorator maps the <kpi-card> HTML tag to this logic block
    [HtmlTargetElement("kpi-card", TagStructure = TagStructure.WithoutEndTag)]
    public class KpiCardTagHelper : BaseComponentTagHelper<KpiCard>
    {
        public KpiCardTagHelper(IHtmlHelper htmlHelper) : base(htmlHelper)
        {
        }
    }
}
