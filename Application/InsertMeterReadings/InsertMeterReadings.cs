﻿namespace Ensek.Energy.Command.Application.InsertMeterReadings
{
    using Ensek.Energy.Command.Application.InsertMeterReadings.Interfaces;
    using Ensek.Energy.Command.Infrastructure.Interfaces;
    using Ensek.Energy.Command.Model;
    using MediatR;
    using System.Collections.Generic;
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
            var meterReadingsToProcess = await _meterReadingsCleansingService.Cleanse(request);
            var processedRows = await _meterReadingsRepository.InsertMeterReadings(meterReadingsToProcess);

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
