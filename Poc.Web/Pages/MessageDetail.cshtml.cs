using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Poc.Data.Application.Services;
using Poc.RCL.Models;

namespace Poc.Web.Pages
{
    public class MessageDetailModel: PageModel
    {
        public MessageDetailDto? Detail { get; private set; }
        private readonly IKpiService _kpiService;

        public MessageDetailModel(IKpiService kpiService)
        {
            _kpiService = kpiService;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var detail = await _kpiService.GetMessageDetailAsync(id);

            if (detail is null)
            {
                return NotFound();
            }

            Detail = detail;
            return Page();
        }

        public async Task<IActionResult> OnGetAttachmentAsync(int id, int index)
        {
            var file = await _kpiService.GetAttachmentFileAsync(id, index);

            if (file is null)
            {
                return NotFound();
            }

            return File(file.Content, file.ContentType, file.FileName);
        }
    }
}
