using BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LatexController : ControllerBase
    {
        private readonly LatexService _latexService;

        public LatexController(LatexService latexService)
        {
            _latexService = latexService;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertLatexToPdf([FromBody] LatexRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.latexCode))
            {
                return BadRequest("LaTeX code dau ?");
            }

            var (pdfData, outputMessage) = await _latexService.ConvertLatextoPdfAsync(request.latexCode);
            if (pdfData == null)
            {
                return BadRequest(new { Message = "Error converting LaTeX to PDF", Details = outputMessage });
            }
            return File(pdfData, "application/pdf", "document.pdf");
        }
    }

    public class LatexRequest
    {
        public string latexCode { get; set; }
    }

}
