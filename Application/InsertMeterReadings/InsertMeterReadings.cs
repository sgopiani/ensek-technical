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
        private IMeterReadingsValidationService _meterReadingsValidationService;

        public InsertMeterReadings(
            IMeterReadingsValidationService meterReadingsValidationService)
        {
            _meterReadingsValidationService = meterReadingsValidationService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
             await _meterReadingsValidationService.Validate(request);

            return default;
        }

        public class Request : IRequest<Response>
        {
            public List<MeterReading> MeterReadings { get; set; }
        }

        public class Response
        {

        }
    }
}
