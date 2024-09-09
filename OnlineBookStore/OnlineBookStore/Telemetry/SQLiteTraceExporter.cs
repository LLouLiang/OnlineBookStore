using OpenTelemetry;
using System.Diagnostics;
using Trace = OnlineBookStore.Models.Trace;

namespace OnlineBookStore.WebApi.Telemetry
{
    public class SQLiteTraceExporter : BaseExporter<Activity>
    {
        private readonly OnlineBookStoreDbContext _dbContext;

        public SQLiteTraceExporter(OnlineBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override ExportResult Export(in Batch<Activity> batch)
        {
            foreach (var activity in batch)
            {
                var trace = new Trace
                {
                    TraceId = activity.TraceId.ToString(),
                    SpanId = activity.SpanId.ToString(),
                    ParentSpanId = activity.ParentSpanId.ToString(),
                    OperationName = activity.DisplayName,
                    StartTime = activity.StartTimeUtc,
                    EndTime = activity.StartTimeUtc + activity.Duration,
                    Status = activity.Status.ToString()
                };

                _dbContext.Traces.Add(trace);
            }

            _dbContext.SaveChanges();
            return ExportResult.Success;
        }
    }
}
