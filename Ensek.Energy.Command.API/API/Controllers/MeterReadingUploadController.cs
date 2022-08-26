namespace Ensek.Energy.Command.API.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("meter-reading-uploads")]
    public class MeterReadingUploadController : Controller
    {
        private IMediator _mediator;

        private IEnumerable<string> AllowedFileTypes = new List<string> {
            ".csv"
        };

        public MeterReadingUploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> UploadMeterReadingsCSV(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (AllowedFileTypes.Any(x => x.Equals(Path.GetExtension(file.FileName), StringComparison.CurrentCultureIgnoreCase)))
            {
                return Ok();

            }

            return BadRequest();
        }
    }
}
