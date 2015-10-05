function Backup-Database($deployFolder, $database)
{
    $backupFolder = "$deployFolder\Backups"
    if (!(Test-Path $backupFolder)) {
        New-Item $backupFolder -Type Directory | Out-Null
    }
    $backup = "{0}\{1}.{2}.bak" -f $backupFolder, $database, [DateTime]::Now.ToString("yyyyMMdd.HHmm")
    
    $query = "
        IF EXISTS (SELECT 1 FROM sys.databases WHERE Name = '$database')
        BEGIN
            BACKUP DATABASE $database 
            TO DISK = '$backup' 

            SELECT 1 AS Existed
        END
        ELSE 
        BEGIN
            SELECT 0 AS Existed
        END
    "
    $result = Invoke-Sqlcmd -Database "master" -Query $query
    Exit-IfError

    if ($result.Existed) {
        Log -level:"success" -message:"Database $database succcessfully backed up to $backup"
    }    
}

function New-Database($database)
{
    $query = "
        IF EXISTS (SELECT 1 FROM sys.databases WHERE Name = '$database')
        BEGIN
            ALTER DATABASE $database SET SINGLE_USER WITH ROLLBACK IMMEDIATE
            DROP DATABASE $database 
        END
        GO

        CREATE DATABASE $database
        GO

        USE $database
        GO

        CREATE TABLE Nodes (
            Id INT CONSTRAINT PK_Nodes PRIMARY KEY NOT NULL,
            Label VARCHAR(50) NOT NULL
        )
        GO

        CREATE TABLE Edges (
            Id INT CONSTRAINT PK_Edges PRIMARY KEY IDENTITY(1,1) NOT NULL,
            [From] INT NOT NULL,
            [To] INT NOT NULL,
            CONSTRAINT [FK_Edges_From] FOREIGN KEY([From]) REFERENCES Nodes (Id),
            CONSTRAINT [FK_Edges_To] FOREIGN KEY([To]) REFERENCES Nodes (Id)
        )
        GO
    "
    Invoke-Sqlcmd -Database "master" -Query $query
    Exit-IfError
        
    Log -level:"success" -message:"Database $database succcessfully created"
}
