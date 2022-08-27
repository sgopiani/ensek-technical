namespace Ensek.Energy.Command.Application.InsertMeterReadings
{
    using Ensek.Energy.Command.Application.InsertMeterReadings.Interfaces;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using static InsertMeterReadings;

    public class InsertMeterReadings : IRequestHandler<Request,Response>
    {
        private IMeterReadingsCleansingService _meterReadingsCleansingService;

        public InsertMeterReadings(
            IMeterReadingsCleansingService meterReadingsValidationService)
        {
            _meterReadingsCleansingService = meterReadingsValidationService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
             var meterReadingsToProcess = await _meterReadingsCleansingService.Cleanse(request);
            //var processedRows = await _meterReadingsRepository.InsertMeterReadings(meterReadingsToProcess);

            return new Response
            {
                
            };
        }

        public class Request : IRequest<Response>
        {
            public List<MeterReading> MeterReadings { get; set; }
        }

        public class Response
        {
            public int Failures { get; set; }
            public IEnumerable<MeterReading> Sucessful { get; set; }
        }
    }
}
