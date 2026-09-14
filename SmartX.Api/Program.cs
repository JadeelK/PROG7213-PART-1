using SmartX.Api.Models;
using SmartX.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Make enums serialize as strings
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddSingleton<SensorRegistry>();
builder.Services.AddSingleton<TelemetryHistoryStore>();
builder.Services.AddAntiforgery();

var app = builder.Build();

app.UseCors("AllowClient");
app.UseAntiforgery();

// Health check
app.MapGet("/api/health", () => Results.Ok(new { status = "Smart-X API is running" }));

// --- Sensor registration & recursive location validation ---
app.MapPost("/api/sensors/register", (SensorRegistration sensor, SensorRegistry registry) =>
{
    var pathSegments = sensor.DeploymentLocation
        .Split('>', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .ToList();

    sensor.LocationValidated = pathSegments.Count > 0 &&
        DeploymentTreeValidator.ValidatePath(DeploymentTreeSeed.Root, pathSegments);

    registry.Add(sensor);
    return Results.Created($"/api/sensors/{sensor.Id}", sensor);
});

app.MapGet("/api/sensors", (SensorRegistry registry) => Results.Ok(registry.All));

// --- File upload ---
app.MapPost("/api/sensors/{id}/upload", async (string id, IFormFile file, SensorRegistry registry) =>
{
    var sensor = registry.Find(id);
    if (sensor is null) return Results.NotFound(new { message = "Sensor not found." });

    var uploadsFolder = Path.Combine(AppContext.BaseDirectory, "UploadedFiles");
    Directory.CreateDirectory(uploadsFolder);

    var safeFileName = $"{id}_{file.FileName}";
    var filePath = Path.Combine(uploadsFolder, safeFileName);

    using (var stream = new FileStream(filePath, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }

    sensor.AttachedFileNames.Add(file.FileName);
    return Results.Ok(sensor);
}).DisableAntiforgery();

// --- Telemetry simulation (jagged arrays → List<T>) ---
app.MapGet("/api/telemetry/seed", (int batches, int maxSensors, TelemetryHistoryStore store) =>
{
    store.SeedMockBatches(batches, maxSensors);
    return Results.Ok(new
    {
        batchSizes = store.GetRawBatches().Select(b => b.Length).ToArray(),
        totalReadings = store.GetProcessedReadings().Count,
        sample = store.GetProcessedReadings().Take(10).Select(p => p.ToString())
    });
});

// --- Generic TelemetryPacket<T> demo ---
app.MapGet("/api/telemetry/mixed-demo", () =>
{
    var floatPacket = new TelemetryPacket<float>("SOIL-01", 42.7f, "%");
    var intPacket = new TelemetryPacket<int>("PWR-01", 1500, "W");
    var boolPacket = new TelemetryPacket<bool>("VALVE-01", true, "");

    return Results.Ok(new
    {
        floatPacket = floatPacket.ToString(),
        intPacket = intPacket.ToString(),
        boolPacket = boolPacket.ToString()
    });
});

// --- Operator overloading demo ---
app.MapGet("/api/telemetry/power-aggregate", () =>
{
    var meter1 = new PowerMeterReading { MeterId = "Meter1", Watts = 1200 };
    var meter2 = new PowerMeterReading { MeterId = "Meter2", Watts = 850 };

    var meter3 = meter1 + meter2;
    var delta = meter1 - meter2;
    var meter1IsHigher = meter1 > meter2;

    return Results.Ok(new
    {
        meter1 = meter1.ToString(),
        meter2 = meter2.ToString(),
        aggregate = meter3.ToString(),
        delta = delta.ToString(),
        meter1IsHigher
    });
});

app.Run();