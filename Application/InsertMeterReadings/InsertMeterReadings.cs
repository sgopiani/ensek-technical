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
             var result = await _meterReadingsCleansingService.Cleanse(request);

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
