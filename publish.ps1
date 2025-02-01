# Usage:
# .\publish.ps1 -Version 1.3.1

param(
    [Parameter(
        Mandatory,
        Position = 0
    )]
    [string]$Version
)

docker build -t neumanna/timesheet:$Version -t neumanna/timesheet:latest ./Timesheet

docker push neumanna/timesheet:$Version
docker push neumanna/timesheet:latest