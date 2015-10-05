param(
    $deployFolder = "C:\GraphDeploy",
    $database = "Graph",
    $serviceName = "Graph Massive Service"
)

Add-PSSnapin SqlServerCmdletSnapin100
Add-PSSnapin SqlServerProviderSnapin100

Import-Module .\_HelpersCommon.psm1
Import-Module .\_HelpersDatabase.psm1
Import-Module .\_HelpersServices.psm1

try {
    Backup-Database
    New-Database

    Install-Service 
}
finally {
    Set-Location $PSScriptRoot
}

