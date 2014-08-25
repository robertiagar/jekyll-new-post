param([switch]$WhatIf = $false)

$installDir = Split-Path $MyInvocation.MyCommand.Path -Parent
$importLine = ". `'${installDir}\import.ps1`'"

function Get-FileEncoding($Path) {
    $bytes = [byte[]](Get-Content $Path -Encoding byte -ReadCount 4 -TotalCount 4)

    if(!$bytes) { return 'utf8' }

    switch -regex ('{0:x2}{1:x2}{2:x2}{3:x2}' -f $bytes[0],$bytes[1],$bytes[2],$bytes[3]) {
        '^efbbbf'   { return 'utf8' }
        '^2b2f76'   { return 'utf7' }
        '^fffe'     { return 'unicode' }
        '^feff'     { return 'bigendianunicode' }
        '^0000feff' { return 'utf32' }
        default     { return 'ascii' }
    }
}

if(!(Test-Path $PROFILE)) {
    Write-Host "Creating PowerShell profile...`n$PROFILE"
    New-Item $PROFILE -Force -Type File -ErrorAction Stop -WhatIf:$WhatIf > $null
}

if(Select-String -Path $PROFILE -Pattern $importLine -Quiet -SimpleMatch) {
    Write-Host "It seems jekyll-new-post is already installed..."
    return
}

Write-Host "Adding jekyll-new-post to profile..."
@"

# Import jekyll-new-post module
$importline

"@ | Out-File $PROFILE -Append -WhatIf:$WhatIf -Encoding (Get-FileEncoding $PROFILE)