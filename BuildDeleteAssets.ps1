# Define paths
$sourceFilePath = "/work/DeleteAssets.cs"
$logFilepath="./Powershell.log"

# test sourceFilePath
try {
    if (!(Test-Path $sourceFilePath)) {
        throw "File '$sourceFilePath' does not exist."
    }
} catch {
    Add-Content -Path $logFilepath -Value "Error: $_"
    Exit 1
}


$destinationDirectory = "/work/src/app/"
$destinationFilePath = "$destinationDirectory/DeleteAssets.dll"

# Define the path to the directory containing the C# source file
$projectDirectory = "/work"

try {
    # Change the current directory to the project directory
    Set-Location $projectDirectory
    
    # Compile the C# source file
    Write-Output "Compiling $sourceFilePath..."
    dotnet build -c Release
    Write-Output "Compilation successful. Output file: $destinationFilePath"
}
catch {
    Write-Error "Failed to compile ${sourceFilePath}: $_"
    Exit 1
}

# Check if destination directory exists, create if it doesn't
if (-not (Test-Path $destinationDirectory -PathType Container)) {
    Write-Output "Creating destination directory: $destinationDirectory"
    New-Item -Path $destinationDirectory -ItemType Directory | Out-Null
}

# Move the compiled DLL to the destination directory
try {
    Write-Output "Moving $destinationFilePath to $destinationDirectory"
    Move-Item -Path $destinationFilePath -Destination $destinationDirectory -Force
    Write-Output "Move successful."
}
catch {
    Write-Error "Failed to move $destinationFilePath to ${destinationDirectory}: $_"
    Exit 1
}
