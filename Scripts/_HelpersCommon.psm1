function Log($level, $message)
{
    if     ($level -eq "info")   { write-host $message }
    elseif ($level -eq "warn")   { write-host $message -foregroundColor Yellow }
    elseif ($level -eq "debug")  { write-host "`t$message" -foregroundColor DarkGray }    
    elseif ($level -eq "success"){ write-host $message -foregroundColor Green }    
    elseif ($level -eq "error")  { write-host $message -foregroundColor Red }
    else                         { write-host $message }
}

function Exit-IfError()
{
    if (($error.Count -gt 0) -or (($LASTEXITCODE -ne 0) -and ($LASTEXITCODE -ne "") -and ($LASTEXITCODE -ne $null))) {
        Log -level:"error" -message:"Failed with error code $LASTEXITCODE."
        if ($error.Count -gt 0) {
                foreach($e in $error) {
                        Log -level:"debug" -message:$e
                }
        }
        throw 
    }    
}

