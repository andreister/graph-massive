param(
    $deployFolder = "C:\GraphDeploy",
    $database = "Graph",
    $serviceName = "GraphMassiveService",
    [switch]$backup,
    [switch]$refresh
)

Add-PSSnapin SqlServerCmdletSnapin100
Add-PSSnapin SqlServerProviderSnapin100

Import-Module .\_HelpersCommon.psm1
Import-Module .\_HelpersDatabase.psm1
Import-Module .\_HelpersServices.psm1

try {
    if ($backup) {
        Backup-Database -deployFolder:$deployFolder -database:$database
    }
    if ($refresh) {
        New-Database -database:$database
    }

    Install-Service -deployFolder:$deployFolder -serviceName:$serviceName
}
finally {
    Set-Location $PSScriptRoot
}

