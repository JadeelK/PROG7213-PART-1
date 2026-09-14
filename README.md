# PROG7213-PART-1
classroom link unavailable


# Smart-X IoT Mesh Ecosystem – Part 1

Smart-X is a hybrid Internet of Things (IoT) data ingestion and validation gateway designed for high-throughput telemetry from distributed microcontrollers (e.g. ESP32).  
This repository contains the complete Part 1 solution: a .NET Minimal API backend and a Blazor WebAssembly frontend.

## Architecture

- **Backend**: ASP.NET Core Minimal API (.NET 8/9)
- **Frontend**: Blazor WebAssembly (standalone)
- **Communication**: HTTP + CORS-enabled JSON API
- **Data storage**: In-memory (simulation)

## Key Features Implemented

- Sensor registration (MAC address, deployment location, category)
- Recursive validation of nested deployment trees (Facility → Zone → Sub-Zone)
- Generic `TelemetryPacket<T>` for type-safe handling of float, int and bool values
- Operator overloading on `PowerMeterReading` (`+`, `-`, `>`, `<`)
- Jagged arrays for historical telemetry batches converted to `List<T>`
- Multipart file upload attached to individual sensors
- Dynamic Network Health Heartbeat (engagement feature) that speeds up when validation health drops
- Clean three-pillar gateway landing page

## Prerequisites

- .NET 8 SDK or .NET 9 SDK
- Visual Studio 2022 (or VS Code + C# Dev Kit)
- Modern browser (Chrome / Edge recommended)

## How to Run

### Option A – Visual Studio (Recommended)

1. Open `SmartX.sln`
2. Right-click the **Solution** → **Set Startup Projects** → **Multiple startup projects**
3. Set both `SmartX.Api` and `SmartX.Client` to **Start**
4. Press **F5**

The API will start on `http://localhost:5109`  
The Client will start on `http://localhost:5228` (or similar)

### Option B – Command Line

```bash
# Terminal 1 – API
cd SmartX.Api
dotnet run

# Terminal 2 – Client
cd SmartX.Client
dotnet run
