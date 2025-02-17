#!/bin/sh

cp /etc/secrets/appsettings.json /app/

# Execute the application
exec dotnet /app/TalStorage.dll