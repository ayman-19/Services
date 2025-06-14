using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Persistence.Data;
using static Services.Shared.ValidationMessages.ValidationMessages;

namespace Services.Persistence.Repositories
{
    public sealed class WorkerServiceRepository
        : Repository<WorkerService>,
            IWorkerServiceRepository
    {
        private readonly ServiceDbContext _context;

        public WorkerServiceRepository(ServiceDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async ValueTask DeleteWorkerFromServiceAsync(
            Guid WorkerId,
            Guid ServiceId,
            CancellationToken cancellationToken
        ) =>
            await _context
                .Set<WorkerService>()
                .Where(ws => ws.WorkerId == WorkerId && ws.ServiceId == ServiceId)
                .ExecuteDeleteAsync(cancellationToken);

        public async ValueTask<double> GetRateAsync(
            Guid workerId,
            CancellationToken cancellationToken
        ) =>
            await _context
                .Set<WorkerService>()
                .Where(ws => ws.WorkerId == workerId)
                .Select(rate => rate.Rate)
                .AverageAsync(rate => rate, cancellationToken);

        public async ValueTask<WorkerService> GetWorkerFromServiceAsync(
            Guid WorkerId,
            Guid ServiceId,
            CancellationToken cancellationToken
        ) =>
            await _context
                .Set<WorkerService>()
                .AsTracking()
                .FirstAsync(ws => ws.WorkerId == WorkerId && ws.ServiceId == ServiceId);

        public async Task RateWorkersAsync(Guid workerId, Guid serviceId)
        {
            var averageRate = await _context
                .Set<Booking>()
                .Where(b =>
                    b.Status == BookingStatus.Completed
                    && b.WorkerId == workerId
                    && b.ServiceId == serviceId
                )
                .AverageAsync(b => (double?)b.Rate);

            if (averageRate == null)
                return;

            var ceilingRate = Math.Ceiling(averageRate.Value);

            await _context
                .Set<WorkerService>()
                .Where(ws => ws.WorkerId == workerId && ws.ServiceId == serviceId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(ws => ws.Rate, ceilingRate));
        }
    }
}
