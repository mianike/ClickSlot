#!/usr/bin/bash

# Check if an argument is provided
if [ -z "$1" ]; then
  echo "Error: Migration name is required."
  echo "Usage: ./add_migration.sh <migration_name>"
  exit 1
fi

thisScriptDir=`dirname $0`
thisScriptDirRP=`realpath $thisScriptDir`

. $thisScriptDir/config.sh

# Run the dotnet ef migrations add command with the provided argument
dotnet ef migrations add -p "$dbContextProject" -s "$startupProject" "$1"
