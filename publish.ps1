param(
    [Parameter(
        Mandatory,
        Position = 0
    )]
    [string]$Version
)

$Env:ContainerImageTags = $Version + ';latest'

dotnet publish /t:PublishContainer -p:Version=$version