#!/usr/bin/bash

thisScriptDir=`dirname $0`
thisScriptDirRP=`realpath $thisScriptDir`

. $thisScriptDir/config.sh

dotnet ef database update -p "$dbContextProject" -s "$startupProject"