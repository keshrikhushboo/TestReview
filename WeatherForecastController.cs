using Microsoft.AspNetCore.Mvc;
using System;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace trace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var span = Tracer.CurrentSpan;

            var telemetryTraceId = span.Context.TraceId.ToString();
            var traceId = Activity.Current?.TraceId.ToString() ?? "No traceId";
            var spanId = Activity.Current?.SpanId.ToString() ?? "No spanId";
            Console.WriteLine($"Telemetry Trace ID: {telemetryTraceId}");
            Console.WriteLine($"System Trace ID: {traceId}");
            Console.WriteLine($"Span ID: {spanId}");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

           
        }
    }
}
