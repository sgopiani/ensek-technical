namespace Ensek.Energy.Command.API.Application.InsertMeterReadings
{
    using Ensek.Energy.Command.API.Application.InsertMeterReadings.Interfaces;
    using Ensek.Energy.Command.API.Infrastructure.Interfaces;
    using Ensek.Energy.Command.API.Model;
    using MediatR;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static InsertMeterReadings;

    public class InsertMeterReadings : IRequestHandler<Request, Response>
    {
        private IMeterReadingsCleansingService _meterReadingsCleansingService;
        private IMeterReadingsRepository _meterReadingsRepository;

        public InsertMeterReadings(
            IMeterReadingsCleansingService meterReadingsValidationService,
            IMeterReadingsRepository meterReadingsRepository)
        {
            _meterReadingsCleansingService = meterReadingsValidationService;
            _meterReadingsRepository = meterReadingsRepository;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            int processedRows = 0;
            IEnumerable<MeterReading> meterReadingsToProcess = request.MeterReadings;

            if (meterReadingsToProcess.Any())
            {
                meterReadingsToProcess = await _meterReadingsCleansingService.Cleanse(request);
            }

            if (meterReadingsToProcess.Any())
            {
                processedRows = await _meterReadingsRepository.InsertMeterReadings(meterReadingsToProcess);
            }

            return new Response
            {
                Sucessful = processedRows,
                Failures = request.MeterReadings.Count - processedRows
            };
        }

        public class Request : IRequest<Response>
        {
            public List<MeterReading> MeterReadings { get; set; }
        }

        public class Response
        {
            public int Failures { get; set; }
            public int Sucessful { get; set; }
        }
    }
}
