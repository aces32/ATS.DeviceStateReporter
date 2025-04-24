#!/bin/bash

SOLUTION="ATS.DeviceStateReporter.sln"
APP_PROJECT="ATS.DeviceStateReporter/ATS.DeviceStateReporter.csproj"

# Check if solution file exists
if [ ! -f "$SOLUTION" ]; then
  echo "Solution file '$SOLUTION' not found!"
  exit 1
fi

# Restore, build, and test the solution
dotnet restore "$SOLUTION"
dotnet build "$SOLUTION" --no-restore
dotnet test "$SOLUTION" --no-build

# initial value to pass to the application
read -p "Enter CSV file path to start the application: " initialValue

# Run the application with the given initial value
dotnet run --project "$APP_PROJECT" -- "$initialValue"
