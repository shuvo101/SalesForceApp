function Get-Platform {
    $platform = Write-Host "Platform: Publish to windows/linux?"
    $platform = Write-Host "a) Windows"
    $platform = Write-Host "b) Linux"
    $platform = Read-Host "Select (a/b):"
    return $platform.Trim()
}

# Main script
try {
    $platform = Get-Platform

    if (($platform -ne "a") -and ($platform -ne "b"))
    {
        Write-Host "Invalid platform option!"
        exit 1
    }

    if ($platform -eq "a")
    {
        $platform = "win"
    }

    if ($platform -eq "b")
    {
        $platform = "linux"
    }

    $timestamp = Get-Date -Format "yyyy-MM-dd__HH-mm-ss"
    $api_foldername = ".\publish\$timestamp-$platform"

    Write-Host "`nExecuting script:"
    Write-Host "dotnet publish .\src\SalesForceApp.API\ -c Release --os $platform -o $api_foldername -f 'net8.0'" -ForegroundColor Blue
    dotnet publish .\src\SalesForceApp.API\ -c Release --os $platform -o $api_foldername -f 'net8.0'

    cd $api_foldername

    if (Test-Path .\appsettings.json) {
      rm .\appsettings.json
    }

    if (Test-Path .\appsettings.Development.json) {
      rm .\appsettings.Development.json
    }

    if (Test-Path .\appsettings.Production.json) {
    } else {
      Write-Host "`nWarning: No appsettings.Production.json found in publish folder" -ForegroundColor Yellow
    }

    cd ..
    cd ..

    Write-Host "Published projects successfully." -ForegroundColor Green
    Write-Host "run 'start $api_foldername' to open publish folder" -ForegroundColor Blue
}
catch {
    Write-Host "`nAn error occurred during the process: $_"
}
