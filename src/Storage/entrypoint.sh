#!/bin/sh
set -ex 

# Ensure /etc/secrets/<filename> exists
if [ -f "/etc/secrets/appsettings.json" ]; then
    echo "Copying appsettings.json from /etc/secrets to the app directory..."
    cp /etc/secrets/appsettings.json /app/appsettings.json
else
    echo "Warning: /etc/secrets/appsettings.json not found!"
fi

# Execute the application
exec dotnet /app/TalStorage.dll