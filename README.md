
# ATS.DeviceStateReporter

A C# console application that reads a state-log CSV file and analyzes a system or device's operational performance — including running time, downtime, availability, and top fault causes.

## Features

- Parses state-logged CSV files (chronological order)
- Calculates:
  - Total running time
  - Total downtime
  - Machine availability (%)
  - Top 5 alarm codes by **total duration**
- Handles exceptions (invalid CSV, unknown states, empty rows)
- Includes full unit, integration, and parser tests
- Built with SOLID, OOP, and clean separation of concerns

## Running the App

### Locally
```bash
dotnet run --project ATS.DeviceStateReporter -- sample-state-log.csv

or 

Simply just run the run-all.sh file and follow the prompts
```

### With Docker
```bash
docker build -f ATS.DeviceStateReporter/Dockerfile -t ats-devicestatereporter-app .
docker run -it ats-devicestatereporter-app sample-state-log.csv
```

## Running Tests

```bash
dotnet test
```

## Project Structure

- `ATS.DeviceStateReporter/` — main console app
- `ATS.DeviceStateReporter.UnitTest/` — unit tests
- `ATS.DeviceStateReporter.IntegrationTest/` — integration tests

---
