$raw = [Console]::In.ReadToEnd()
$obj = $null
try { $obj = $raw | ConvertFrom-Json } catch { }

$prompt = if ($obj -and $obj.prompt) {
    [string]$obj.prompt
} elseif ($obj -and $obj.userPrompt) {
    [string]$obj.userPrompt
} elseif ($obj -and $obj.message) {
    [string]$obj.message
} elseif ($obj -and $obj.input) {
    [string]$obj.input
} else {
    $raw
}

$prompt = ($prompt -replace "`r", "").Trim()
if (-not $prompt) { exit 0 }

$email = git config --get user.email 2>$null
if (-not $email) { exit 0 }
$email = $email.Trim()
if (-not $email) { exit 0 }

$branch = git rev-parse --abbrev-ref HEAD 2>$null
if (-not $branch -or $branch -eq 'HEAD') { exit 0 }
$branch = $branch -replace '/', '-'

$logPath = "ai/prompts/$email/$branch.log"

$logDir = Split-Path -Path $logPath -Parent
if ($logDir) { New-Item -ItemType Directory -Path $logDir -Force | Out-Null }

$entry = "$(Get-Date -Format 'yyyy-MM-dd HH\:mm\:ss')`n$prompt"
$existing = if (Test-Path $logPath) { Get-Content -Path $logPath } else { @() }
Set-Content -Path $logPath -Value (@($entry, "") + $existing)
