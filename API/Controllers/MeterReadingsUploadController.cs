namespace Ensek.Energy.Command.API.Controllers
{
    using CsvHelper;
    using Ensek.Energy.Command.API.Mappers;
    using Ensek.Energy.Command.Application.InsertMeterReadings;
    using Ensek.Energy.Command.Model;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("meter-reading-uploads")]
    public class MeterReadingsUploadController : Controller
    {
        private IMediator _mediator;
        private IFactory _csvFactory;

        private IEnumerable<string> AllowedFileTypes = new List<string> {
            ".csv"
        };

        public MeterReadingsUploadController(IMediator mediator, IFactory csvFactory)
        {
            _mediator = mediator;
            _csvFactory = csvFactory;
        }

        [HttpPost]
        public async Task<IActionResult> UploadMeterReadingsCSV(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!AllowedFileTypes.Any(x => x.Equals(Path.GetExtension(file.FileName), StringComparison.CurrentCultureIgnoreCase)))
            {
                return new UnsupportedMediaTypeResult();

            }

            var insertMeterReadingsRequest = new InsertMeterReadings.Request
            {
                MeterReadings = DeserialiseMeterReadings(file)
            };

            var response = await _mediator.Send(insertMeterReadingsRequest);

            return Ok(response);

        }

        private List<MeterReading> DeserialiseMeterReadings(IFormFile file)
        {
            var meterReadings = new List<MeterReading>();


            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = _csvFactory.CreateReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<MeterReadingCsvMapper>();
                meterReadings = csv.GetRecords<MeterReading>().ToList();
            };

            return meterReadings;
        }
    }
}
