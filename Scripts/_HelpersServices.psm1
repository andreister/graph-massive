function Install-Service($deployFolder, $serviceName)
{
    $copyFrom = Resolve-Path "..\Graph.Services\bin\Debug"
    $copyTo = "$deployFolder\Service"

    $service = Get-Service $serviceName -ErrorAction SilentlyContinue
    if (!($service -eq $null)) {
        Stop-Service -Force -Name $serviceName
    }

    if (Test-Path $copyTo) {
        Remove-Item $copyTo -Recurse
    }
    New-Item $copyTo -Type Directory | Out-Null
    Copy-Item -Path $copyFrom\* -Destination $copyTo -Recurse -Exclude "\Logs\*"

    if ($service -eq $null) {
        $user = New-Object Security.Principal.WindowsPrincipal( [Security.Principal.WindowsIdentity]::GetCurrent() )
        if (-not $user.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
            Log -level "error" -message "'$serviceName' service did not exist before - to create it please run the script as Administrator"
            Exit
        }
        $executable = "$copyTo\Graph.Services.exe"
        $service = New-Service -Name $serviceName -BinaryPathName $executable -DisplayName $serviceName -StartupType Automatic | Out-Null
    }

    if ($service -eq $null) {
        Log -level:"warn" -message:"Failed to update service '$serviceName'"
    }
    else {
        Log -level:"success" -message:"Service '$serviceName' succcessfully updated"
    }

}